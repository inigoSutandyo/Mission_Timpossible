using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations.Rigging;
using UnityEditor.Animations;
using System;

public class PlayerWeapon : MonoBehaviour
{
    public RaycastWeapon weapon;
    public List<RaycastWeapon> weaponList;

    public QuestManager questManager;
    public Text ammoText;
    public Transform crossHairTaget;
    public Transform primaryParent, secondaryParent;

    public Transform leftHandle;
    public Transform rightHandle;

    [SerializeField] Rig rigHands;
    public Animator rigController;

    private bool equipRifle = false , equipPistol = false;
    private string currWeapon = "None";

    public Cinemachine.CinemachineFreeLook freeLook;

    private bool isReloading;
    void Start()
    {
        ammoText.text = "";

        weaponList.Add(null);
        weapon = null;
        RaycastWeapon currWeapon = GetComponentInChildren<RaycastWeapon>();

        if (currWeapon != null)
        {
            EquipWeapon(currWeapon);
        }
    }

    // Update is called once per frame
    void Update()
    {
        BulletUpdater();

        if (isReloading) return;

        ChangeWeaponInitiator();
        if (weapon)
        {
            AutoReload();
            WeaponController();
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
        else if (weapon.isShooting)
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
        //print("Reload");
        StartCoroutine(StartReloadWeapon());
        //rigController.SetTrigger("Reload");
        weapon.Reload();
        weapon.UpdateText();
        //StartCoroutine(StopReloadWeapon());
    }

    private void ChangeWeapon(string name)
    {
        if (currWeapon != name)
        {
            StartCoroutine(HolsterWeapon(currWeapon));

            if (name == "None" )
            {
                ammoText.text = "";
                weapon = weaponList[0];
                currWeapon = name;
                return;
            }

            foreach (var w in weaponList)
            {
                if (w && w.gameObject.tag == name)
                {
                    weapon = w;
                }
            }

            // play animation to equip weapon
            if (weapon)
            {
                StartCoroutine(EquipWeapon(name));
            }
            currWeapon = name;
        }
        //print(currWeapon);
    }

    private IEnumerator EquipWeapon(string name)
    {
        weapon.UpdateText();
        rigController.SetBool("Holster " + name, false);
        //rigController.Play("equip_" + name);
        do
        {
            yield return new WaitForEndOfFrame();
        } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.5f);
        
    }

    private IEnumerator StopReloadWeapon()
    {
        rigController.SetBool("Reload", false);
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
        }
        

    }

    public void EquipWeapon(RaycastWeapon newWeapon)
    {
        weapon = newWeapon;
        weapon.raycastTarget = crossHairTaget;
        weapon.questManager = questManager;
        weapon.ammoText = ammoText;
        weapon.weaponRecoil.freeLook = freeLook;

        if (newWeapon.tag == "Pistol")
        {
            equipPistol = true;
            weapon.transform.parent = secondaryParent;
        } else
        {
            equipRifle = true;
            weapon.transform.parent = primaryParent;
        }
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        weaponList.Add(weapon);

        ChangeWeapon(newWeapon.tag);
        
        
        //weapon.gameObject.SetActive(true);
    }
    

}
