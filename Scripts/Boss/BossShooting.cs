using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShooting : MonoBehaviour
{
    public bool isShooting = false;

    private BossBehaviour behaviour;
    [SerializeField] private ParticleSystem[] muzzleFlash;
    [SerializeField] private Transform raycastOrigin;
    public Transform raycastTarget;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] TrailRenderer bulletTrail;

    private Ray ray;
    private RaycastHit hitTarget;
    private float time;

    List<Bullet> bullets = new List<Bullet>();
    private float bulletLifetime = 1.0f;

    private PlayerStatus playerStatus;
    [SerializeField] private AudioSource shootAudio;
    private AudioManager audioManager;
    private void Awake()
    {
        playerStatus = FindObjectOfType<PlayerStatus>();
        behaviour = GetComponent<BossBehaviour>();

        audioManager = new AudioManager(shootAudio);
    }

    Bullet CreateBullet(Vector3 pos, Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.initPos = pos;
        bullet.initVelocity = velocity;
        bullet.time = 0.0f;
        bullet.trailer = Instantiate(bulletTrail, pos, Quaternion.identity);
        bullet.trailer.AddPosition(pos);
        return bullet;
    }

    public void Shoot()
    {
        isShooting = true;
        time = 0f;

    }

    public void UpdateShoot(float deltaTime)
    {
        time += deltaTime;
        float interval = 1.0f / behaviour.fireRate;

        while (time >= 0.0f)
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

        Vector3 bulletVelocity = (raycastTarget.position - raycastOrigin.position).normalized * behaviour.bulletSpeed;
        var bullet = CreateBullet(raycastOrigin.position, bulletVelocity);
        bullets.Add(bullet);
    }

    public bool CheckBullet()
    {
        if (bullets.Count > 0)
        {
            return true;
        }
        return false;
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

    Vector3 GetPosition(Bullet bullet)
    {
        Vector3 gravity = Vector3.down * behaviour.bulletDrop;
        return bullet.initPos + bullet.initVelocity * bullet.time + 0.5f * gravity * bullet.time * bullet.time;
    }

    private void SimulateBullet(float deltaTime)
    {
        bullets.ForEach(bullet =>
        {

            Vector3 currPos = GetPosition(bullet);
            bullet.time += Time.deltaTime;
            Vector3 newPos = GetPosition(bullet);
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
            hitEffect.transform.position = hitTarget.point;
            hitEffect.transform.forward = hitTarget.normal;
            hitEffect.Emit(1);

            if (hitTarget.transform.CompareTag("Player"))
            {
                playerStatus.TakeDamage(behaviour.bulletDamage);
            }

            b.trailer.transform.position = hitTarget.point;
            b.time = bulletLifetime;
        }
        else
        {
            b.trailer.transform.position = end;
        }
    }


    public void Stop()
    {
        isShooting = false;
    }
}
