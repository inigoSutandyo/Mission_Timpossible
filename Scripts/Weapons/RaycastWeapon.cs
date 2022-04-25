using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastWeapon : MonoBehaviour
{
    public bool isShooting = false;
    private WeaponBehaviour currWeapon;
    [SerializeField] private ParticleSystem[] muzzleFlash;
    [SerializeField] private Transform raycastOrigin;
    public Transform raycastTarget;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] TrailRenderer bulletTrail;

    private Ray ray;
    private RaycastHit hitTarget;
    private float time;
    
    List<Bullet> bullets = new List<Bullet>();
    private float bulletLifetime = 3.0f;

    public int ammoMax;
    public int ammoTotal;
    public int ammoCount;
    public int magazineMax;

    public Text ammoText;

    private PlayerStatus playerStatus;
    private int questTarget = 0;
    private int questShoot = 0;

    public QuestManager questManager;

    public RecoilWeapon weaponRecoil;

    public GameObject magazine;

    private AudioSource shootAudio;
    private AudioManager audioManager;
    private void Awake()
    {
        shootAudio = GetComponent<AudioSource>();

        currWeapon = GetComponent<WeaponBehaviour>();
        weaponRecoil = GetComponent<RecoilWeapon>();
        
        audioManager = new AudioManager(shootAudio);
    }

    void Start()
    {
        ammoMax = currWeapon.magazine * currWeapon.ammoCount;
        magazineMax = currWeapon.ammoCount;
        ammoCount = 0;
        ammoTotal = 0;

        playerStatus = GetComponentInParent<PlayerStatus>();
    }

    Vector3 getPosition(Bullet bullet)
    {
        Vector3 gravity = Vector3.down * currWeapon.bulletDrop;
        return bullet.initPos + bullet.initVelocity * bullet.time + 0.5f * gravity * bullet.time * bullet.time;
    }

    Bullet CreateBullet(Vector3 pos, Vector3 velocity)
    {
        Bullet bullet = new Bullet
        {
            initPos = pos,
            initVelocity = velocity,
            time = 0.0f,
            trailer = Instantiate(bulletTrail, pos, Quaternion.identity)
        };
        bullet.trailer.AddPosition(pos);
        return bullet;
    }

    public void Shoot()
    {
        if (ammoCount > 0)
        {
            audioManager.PlayAudioWithoutLoop();
            isShooting = true;
            time = 0f;
            //BulletFire();
        }
        
    }

    public void UpdateText()
    {
        ammoText.text = ammoCount.ToString() + "/" + ammoTotal.ToString();
    }
    
    public void Reload()
    {
        if (!CanReload())
        {
            return;
        }
        
        int toAdd = magazineMax - ammoCount;
        if (ammoTotal >= toAdd)
        {
            ammoCount += toAdd;
            ammoTotal -= toAdd;
        }
        else
        {
            ammoCount += ammoTotal;
            ammoTotal -= ammoTotal;
        }

        UpdateText();
    }
    
    public bool CanReload()
    {
        if (ammoCount == magazineMax || ammoTotal == 0)
        {
            return false;
        }

        return true;
    }

    public void UpdateShoot(float deltaTime)
    {
        time += deltaTime;
        float interval = 1.0f / currWeapon.fireRate;
        //audioManager.PlayAudio();
        while (time >= 0.0f && ammoCount > 0)
        {
            BulletFire();
            time -= interval;
        }
    }

    private void BulletFire()
    {
        audioManager.PlayAudioWithoutLoop();

        foreach (var part in muzzleFlash)
        {
            part.Emit(1);
        }

        Vector3 bulletVelocity = (raycastTarget.position - raycastOrigin.position).normalized * currWeapon.bulletSpeed;
        var bullet = CreateBullet(raycastOrigin.position, bulletVelocity);
        bullets.Add(bullet);
        ammoCount -= 1;

        // quest shoot 50 times with a rifle
        if (playerStatus.quest.id == 5 && transform.tag == "Rifle")
        {
            questShoot += 1;
            questManager.UpdateQuestBox(questShoot.ToString() + " / 50");
            //print(questShoot.ToString() + " / 50");
            if (questShoot >= 50)
            {
                playerStatus.quest.isDone = true;
            }
        }

        UpdateText();

        weaponRecoil.GenerateRecoil();
    }

    public void UpdateBullet(float deltaTime)
    {
        SimulateBullet(deltaTime);
        DestroyBullet();
    }

    private void DestroyBullet()
    {
        bullets.RemoveAll(bullet => bullet.time >= bulletLifetime);
    }

    private void SimulateBullet(float deltaTime)
    {
        bullets.ForEach(bullet =>
        {
            
            Vector3 currPos = getPosition(bullet);
            bullet.time += Time.deltaTime;
            Vector3 newPos = getPosition(bullet);
            RaycastSegment(currPos, newPos, bullet);
        });
    }

    void RaycastSegment(Vector3 start, Vector3 end, Bullet b)
    {
        Vector3 dir = end - start;
        float dist = dir.magnitude;
        ray.origin = start;
        ray.direction = dir;

        if (b == null)
        {
            return;
        }

        if (Physics.Raycast(ray, out hitTarget, dist))
        {
            //Debug.Log(hitTarget.transform.name);

            // if hit target range --> for quest 3 
            if (hitTarget.transform.parent != null && hitTarget.transform.parent.transform.name == "Target")
            {
                
                if (playerStatus.quest.id == 3)
                {
                    //print("Hit Target!!");
                    
                    questTarget += 1;
                    questManager.UpdateQuestBox(questTarget.ToString() + " / 10");
                    if (questTarget >= 10)
                    {
                        playerStatus.quest.isDone = true;
                    }
                }
            }

            if (hitTarget.transform.CompareTag("Enemy"))
            {
                EnemyBehaviour enemy = hitTarget.transform.GetComponent<EnemyBehaviour>();
                enemy.TakeDamage(currWeapon.bulletDamage);
            } else if (hitTarget.transform.CompareTag("Boss"))
            {
                BossBehaviour boss = hitTarget.transform.GetComponent<BossBehaviour>();
                boss.TakeDamage(currWeapon.bulletDamage);
            }


            hitEffect.transform.position = hitTarget.point;
            hitEffect.transform.forward = hitTarget.normal;
            hitEffect.Emit(1);

            b.trailer.transform.position = hitTarget.point;
            b.time = bulletLifetime;
        } else
        {
            b.trailer.transform.position = end;
        }
    }


    public void Stop()
    {
        isShooting = false;
        //audioManager.StopAudio();
    }
}
