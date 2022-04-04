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

    private PlayerStatus playerStatus;
    private PlayerWeapon playerWeapon;
    [SerializeField] private QuestManager questManager;
    [SerializeField] private GameObject infoPopup;

    [SerializeField] private Camera characterCam;

    [SerializeField] private RaycastWeapon rifleWeapon;
    [SerializeField] private RaycastWeapon pistolWeapon;


    //private PlayerAiming playerAiming;

    private bool isDialouge = false;

    void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        playerWeapon = GetComponent<PlayerWeapon>();
        //playerAiming.currentWeapon = null;
        
        setFalse();
    }

    void Update()
    {
        if (isDialouge)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                StopDialouge();
            }
        }
    }

    private void StopDialouge()
    {
        isDialouge = false;
        //print("NEXT!!!");
        Time.timeScale = 1;
        questManager.CloseDialouge();
    }

    private void StartDialogue(int index)
    {
        isDialouge = true;
        infoPopup.SetActive(false);
        Time.timeScale = 0;
        questManager.ShowDialogue(index);
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
                weaponTrigger();
            }
        }
        else
        {
            infoPopup.SetActive(false);
        }
    }


    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isNPC)
            {
                infoPopup.SetActive(false);
                isTrigger = false;
                
                if (playerStatus.quest.id == 1 && playerStatus.quest.isDone == false)
                {
                    StartDialogue(playerStatus.quest.id - 1);
                    questManager.newQuest();
                } else if (playerStatus.quest.isDone == true)
                {
                    StartDialogue(playerStatus.quest.id - 1);
                    questManager.newQuest();
                } else
                {
                    // interact talk something aa
                }
            }
            // When player is in second quest cannot pickup rifle
            else if (isPistol)
            {
                infoPopup.SetActive(false);

                // Equip weapon here
                RaycastWeapon newWeapon = Instantiate(pistolWeapon);
                playerWeapon.EquipWeapon(newWeapon);
                pistolWeapon = playerWeapon.weapon;

                // remove pistol from scene
                //triggerObj.SetActive(false);
                //triggerObj.GetComponent<BoxCollider>().enabled = false;

                Destroy(triggerObj);

                setFalse();

                // Quest
                playerStatus.quest.isDone = true;

            } else if (isRifle)
            {
                RaycastWeapon newWeapon = Instantiate(rifleWeapon);
                playerWeapon.EquipWeapon(newWeapon);
                rifleWeapon = playerWeapon.weapon;

                // remove rifle from scene
                triggerObj.SetActive(false);
                triggerObj.GetComponent<BoxCollider>().enabled = false;

                setFalse();

                playerStatus.quest.isDone = true;
            } else if (playerStatus.quest.id > 1)
            {
                var ray = characterCam.ViewportPointToRay(Vector3.one * 0.5f);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 10f))
                {
                    print(hit.transform.name);
                    if (hit.transform.tag == "Ammo B")
                    {
                        if (rifleWeapon.ammoTotal + 120 <= rifleWeapon.ammoMax)
                        {
                            rifleWeapon.ammoTotal += 120;
                            rifleWeapon.UpdateText();
                        }
                        Destroy(hit.transform.gameObject);
                    } else if (hit.transform.tag == "Ammo A")
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
        }
    }

 

    private void weaponTrigger()
    {
        infoPopup.SetActive(true);
     
        infoPopup.GetComponentInChildren<Text>().text = "Press F to pickup weapon.";
    }


    private void NPCTrigger()
    {
        infoPopup.SetActive(true);
        infoPopup.GetComponentInChildren<Text>().text = "Press F to talk to Asuna.";
        
    }


    private void OnTriggerEnter(Collider other)
    {
        setFalse();
        
        if (other.tag == "NPC")
        {
            isTrigger = true;
            isNPC = true;
            triggerObj = other.gameObject;

        }  else if (other.tag == "Pistol" && playerStatus.quest.id == 2)
        {
            isTrigger = true;
            isPistol = true;
            triggerObj = other.gameObject;
        } else if (other.tag == "Rifle" && playerStatus.quest.id == 4)
        {
            isTrigger = true;
            isRifle = true;
            triggerObj = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        setFalse();
    }

    private void setFalse()
    {
        isTrigger = false; 
        isNPC = false;
        isPistol = false;
        isRifle = false;

        triggerObj = null;
    }
}
