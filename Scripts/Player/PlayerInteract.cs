using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    private GameObject triggerObj;
    private bool isTrigger, isNPC;
    private bool isPistol, isRifle;
    private bool isTeleporter;

    private PlayerStatus playerStatus;
    private PlayerWeapon playerWeapon;
    [SerializeField] private QuestManager questManager;
    [SerializeField] private GameObject infoPopup;

    [SerializeField] private Camera characterCam;

    [SerializeField] private RaycastWeapon rifleWeapon;
    [SerializeField] private RaycastWeapon pistolWeapon;

    [SerializeField] private float forceMagnitude;

    [SerializeField] GameObject bossSpawn;

    //private PlayerAiming playerAiming;

    private bool isQuestDialouge = false;
    private bool isIdleDialouge = false;


    private TimelineManager timeline;
    private PauseManager pause;

    private float time = 0f;

    private void Awake()
    {
        playerStatus = GetComponent<PlayerStatus>();
        playerWeapon = GetComponent<PlayerWeapon>();
        timeline = FindObjectOfType<TimelineManager>();
        pause = FindObjectOfType<PauseManager>();
    }

    void Start()
    {
        //playerAiming.currentWeapon = null;
        SetFalse();
    }

    void Update()
    {
        if (pause.isPause) return;
        if (isQuestDialouge)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                StopDialouge();
            }
        }

        if (isIdleDialouge)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                StopIdleDialouge();
            }
        }
    }

    private void StopIdleDialouge()
    {
        isIdleDialouge = false;
        Time.timeScale = 1;
        questManager.CloseDialouge();
    }

    private void StopDialouge()
    {
        isQuestDialouge = false;
        Time.timeScale = 1;
        questManager.CloseDialouge();

        if (playerStatus.quest.id != 5)
        {
            timeline.PlayNextTimeline();
        }


    }

    private void StartDialogue(int index)
    {
        isQuestDialouge = true;
        infoPopup.SetActive(false);
        Time.timeScale = 0;
        questManager.ShowDialogue(index);
    }

    private void StartDialogueIdle()
    {
        isIdleDialouge = true;
        infoPopup.SetActive(false);
        Time.timeScale = 0;
        questManager.ShowIdleDialogue();
    }

    void FixedUpdate()
    {
        if (isTrigger)
        {
            if (isNPC)
            {
                NPCTrigger();
            } else if (isPistol || isRifle && playerStatus.quest.id > 1)
            {
                WeaponTrigger();
            }
        }
        else
        {
            infoPopup.SetActive(false);
        }
    }


    private void LateUpdate()
    {
        time += 1.0f * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isNPC)
            {
                InteractWithNpc();
            } else if (isPistol)
            {
                PickupPistol();
            } else if (isRifle)
            {
                PickupRifle();
            } else if (playerStatus.quest.id > 1)
            {
                PickupAmmo();
            } else if (isTeleporter)
            {
                TeleporterInteraction();
            }
        }
    }

    private void TeleporterInteraction()
    {
        infoPopup.SetActive(false);
        SetFalse();

        // Teleport
        playerStatus.health = 100;
        transform.parent = bossSpawn.transform;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

    }

    private void InteractWithNpc()
    {
        infoPopup.SetActive(false);
        isTrigger = false;

        if (playerStatus.quest.id == 1 || playerStatus.quest.isDone == true && playerStatus.quest.id < 7)
        {
            StartDialogue(playerStatus.quest.id - 1);
            questManager.NewQuest();
            time = 0f;
        }
        else if (time >= 3.0f)
        {
            time = 0f;
            StartDialogueIdle();
        }
    }

    private void PickupPistol()
    {
        infoPopup.SetActive(false);

        // Equip weapon here
        RaycastWeapon newWeapon = Instantiate(pistolWeapon);
        playerWeapon.EquipWeapon(newWeapon);
        pistolWeapon = playerWeapon.weapon;


        Destroy(triggerObj);

        SetFalse();

        // Quest
        playerStatus.quest.isDone = true;
    }

    private void PickupRifle()
    {
        RaycastWeapon newWeapon = Instantiate(rifleWeapon);
        playerWeapon.EquipWeapon(newWeapon);
        rifleWeapon = playerWeapon.weapon;

        // remove rifle from scene
        triggerObj.SetActive(false);
        triggerObj.GetComponent<BoxCollider>().enabled = false;

        SetFalse();

        playerStatus.quest.isDone = true;
    }

    private void PickupAmmo()
    {
        var ray = characterCam.ViewportPointToRay(Vector3.one * 0.5f);

        if (Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            print(hit.transform.name);
            if (hit.transform.CompareTag("Ammo B"))
            {
                if (rifleWeapon.ammoTotal + 120 <= rifleWeapon.ammoMax)
                {
                    rifleWeapon.ammoTotal += 120;
                    rifleWeapon.UpdateText();
                }
                Destroy(hit.transform.gameObject);
            }
            else if (hit.transform.CompareTag("Ammo A"))
            {
                if (pistolWeapon.ammoTotal + 14 <= pistolWeapon.ammoMax)
                {
                    pistolWeapon.ammoTotal += 14;
                    pistolWeapon.UpdateText();
                }
                Destroy(hit.transform.gameObject);
            }

        }
    }
 

    private void WeaponTrigger()
    {
        infoPopup.SetActive(true);
     
        infoPopup.GetComponentInChildren<Text>().text = "Press F to pickup weapon.";
    }


    private void NPCTrigger()
    {
        infoPopup.SetActive(true);
        playerWeapon.Disarm();
        infoPopup.GetComponentInChildren<Text>().text = "Press F to talk to Asuna.";
        
    }


    private void OnTriggerEnter(Collider other)
    {
        SetFalse();
        
        if (other.CompareTag("NPC"))
        {
            isNPC = true;
            TriggerObject(other);
        }  else if (other.CompareTag("Pistol") && playerStatus.quest.id == 2)
        {
            isPistol = true;
            TriggerObject(other);
        } else if (other.CompareTag("Rifle") && playerStatus.quest.id == 4)
        {
            isRifle = true;
            TriggerObject(other);
        } else if (other.CompareTag("Teleporter"))
        {
            isTeleporter = true;
            TriggerObject(other);
        }
    }

    private void TriggerObject(Collider other)
    {
        isTrigger = true;
        triggerObj = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        SetFalse();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!hit.transform.CompareTag("Ammo A") && !hit.transform.CompareTag("Ammo B")) return;

        
        Rigidbody rigidbody = hit.collider.attachedRigidbody;

        if (rigidbody == null)
        {
            return;
        }

        var forceDirection = hit.gameObject.transform.position - transform.position;
        forceDirection.y = 0;
        forceDirection.Normalize();
        rigidbody.AddForceAtPosition(forceDirection * forceMagnitude, transform.position, ForceMode.Impulse);
    }

    private void SetFalse()
    {
        isTrigger = false; 
        isNPC = false;
        isPistol = false;
        isRifle = false;
        isTeleporter = false;
        triggerObj = null;
    }
}
