using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public int health = 100;
    public float noticeRadius = 35.0f, attackRadius = 25.0f;
    public float movementSpeed = 5.0f;
    public int bulletDamage = 1;
    public float bulletSpeed = 800f;
    public float bulletDrop = 20f;
    public float fireRate = 14f;

    [SerializeField] private GameObject AmmoA;
    [SerializeField] private GameObject AmmoB;
    private QuestManager quest;
    private System.Random rand = new System.Random();

    private void Awake()
    {
        quest = FindObjectOfType<QuestManager>();
    }

    private void Update()
    {
        
        if (health <= 0)
        {
            DropAmmo();
            Destroy(this.gameObject);
            quest.AddKillCount();
        }
    }

    private void DropAmmo()
    {
        int x = rand.Next(2);
        //print(x);
        if (x == 0)
        {
            Instantiate(AmmoA, new Vector3(transform.position.x,1,transform.position.z), Quaternion.identity);
        } else
        {
            Instantiate(AmmoB, new Vector3(transform.position.x, 1, transform.position.z), Quaternion.identity);
        }
    }

    public void TakeDamage(int bulletDamage)
    {DropAmmo();
        health -= bulletDamage;
    }
}
