using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations.Rigging;
using System;

public class PlayerWeapon : MonoBehaviour
{
    public RaycastWeapon weapon;
    public List<RaycastWeapon> weaponList;

    public QuestManager questManager;
    public Text ammoText;
    public Transform crossHairTaget;
    public Transform primaryParent, secondaryParent;

    //public Transform leftHandle;
    //public Transform rightHandle;

    //[SerializeField] Rig rigHands;
    public Animator rigController;

    private bool equipRifle = false, equipPistol = false;
    private string currWeapon = "None";

    public Cinemachine.CinemachineFreeLook freeLook;
    private Cinemachine.CinemachineComposer composer;

    private bool isReloading;

    [SerializeField] Image rifleImage;
    [SerializeField] Image pistolImage;

    private PauseManager pause;

    private void Awake()
    {
        pause = FindObjectOfType<PauseManager>();
    }

    void Start()
    {
        ammoText.text = "";

        weaponList.Add(null);
        weapon = null;
        //rigHands.weight = 0;
        RaycastWeapon currWeapon = GetComponentInChildren<RaycastWeapon>();

        if (currWeapon != null)
        {
            EquipWeapon(currWeapon);
        }
        freeLook.m_CommonLens = true;
    }

    // Update is called once per frame
    void Update()
    {
        BulletUpdater();
        ChangeWeaponInitiator();

        if (pause.isPause) return;
        if (isReloading) return;

        if (weapon)
        {
            AutoReload();
            WeaponController();
            AimController();
        }
    }

    private void AimController()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            freeLook.m_Lens.FieldOfView = 20;
        } 

        if (Input.GetButtonUp("Fire2"))
        {
            freeLook.m_Lens.FieldOfView = 40;
        }
    }

    private void BulletUpdater()
    {
        foreach (var w in weaponList)
        {
            if (w)
            {
                w.UpdateBullet(Time.deltaTime);
            }

        }
    }

    private void WeaponController()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            weapon.Shoot();
        }
        if (weapon.isShooting)
        {
            weapon.UpdateShoot(Time.deltaTime);
        }

        if (Input.GetButtonUp("Fire1"))
        {
            weapon.Stop();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            weapon.Stop();
            InitiateReload();
        }

    }

    private void ChangeWeaponInitiator()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && equipRifle)
        {
            ChangeWeapon("Rifle");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && equipPistol)
        {
            ChangeWeapon("Pistol");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeWeapon("None");
        }
    }

    private void AutoReload()
    {
        if (weapon.ammoCount == 0 && weapon.ammoTotal > 0)
        {
            weapon.Stop();
            InitiateReload();
        }
    }

    private void InitiateReload()
    {
        if (!weapon.CanReload())
        {
            return;
        }
        StartCoroutine(StartReloadWeapon());
        weapon.Reload();
        weapon.UpdateText();
    }

    private void ChangeWeapon(string name)
    {
        if (currWeapon != name)
        {
            StartCoroutine(HolsterWeapon(currWeapon));
            
            if (name == "None")
            {
                ammoText.text = "";
                weapon = weaponList[0];
                currWeapon = name;

                pistolImage.color = new Color(0f, 0f, 0f, .7f);
                rifleImage.color = new Color(0f, 0f, 0f, .7f); ;
                return;
            }

            foreach (var w in weaponList)
            {
                if (w && w.gameObject.CompareTag(name))
                {
                    if (name == "Rifle")
                    {
                        rifleImage.color = new Color(0.7f, 0.7f, 0.7f, .5f);

                        pistolImage.color = new Color(0f, 0f, 0f, .7f);
                    } else
                    {
                        pistolImage.color = new Color(0.7f, 0.7f, 0.7f, .5f);

                        rifleImage.color = new Color(0f, 0f, 0f, .7f);
                    }
                    weapon = w;
                }
            }

            // play animation to equip weapon
            if (weapon)
            {
                StartCoroutine(EquipWeaponAnimation(name));
            }
            currWeapon = name;
        }
    }

    private IEnumerator EquipWeaponAnimation(string name)
    {
        //rigHands.weight = 1;
        weapon.UpdateText();
        rigController.SetBool("Holster " + name, false);

        do
        {
            yield return new WaitForEndOfFrame();
        } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.5f);

    }

    private IEnumerator StartReloadWeapon()
    {
        isReloading = true;
        rigController.SetTrigger("Reload");
        yield return new WaitForSeconds(2.0f);
        isReloading = false;
    }
    private IEnumerator HolsterWeapon(string currEquiped)
    {
        if (currEquiped != "None")
        {
            bool holstered = rigController.GetBool("Holster " + currEquiped);

            if (!holstered)
            {
                rigController.SetBool("Holster " + currEquiped, !holstered);
            }

            do
            {
                yield return new WaitForEndOfFrame();
            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.5f);
            //rigHands.weight = 0;
        }
    }

    public void EquipWeapon(RaycastWeapon newWeapon)
    {
        weapon = newWeapon;
        weapon.raycastTarget = crossHairTaget;
        weapon.questManager = questManager;
        weapon.ammoText = ammoText;
        weapon.weaponRecoil.freeLook = freeLook;

        if (newWeapon.CompareTag("Pistol"))
        {
            equipPistol = true;
            weapon.transform.parent = secondaryParent;
        }
        else
        {
            equipRifle = true;
            weapon.transform.parent = primaryParent;
        }
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        weaponList.Add(weapon);

        ChangeWeapon(newWeapon.tag);
    }

    public void Disarm()
    {
        ChangeWeapon("None");
    }

    public bool CheckHoldingWeapon()
    {
        if (currWeapon == "None")
        {
            return false;
        }

        return true;
    }
}