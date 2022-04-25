using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{

    public int health = 2000;    
    public float movementSpeed = 8.0f;
    public int bulletDamage = 1;
    public float bulletSpeed = 800f;
    public float bulletDrop = 20f;
    public float fireRate = 14f;
    public HealthScript HealthBar;
    [SerializeField] private Animator anim;

    [HideInInspector] public bool isDead = false;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {

        if (health <= 0 && !isDead)
        {
            isDead = true;
            // play anim
            StartCoroutine(StartDeathAnimation());
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    private IEnumerator StartDeathAnimation()
    {
        print("Death animation");
        anim.SetTrigger("isDead");
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
}
