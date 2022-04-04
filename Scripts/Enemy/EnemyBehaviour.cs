using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public int health = 100;
    public float noticeRadius = 35.0f, attackRadius = 25.0f;
    public float movementSpeed = 5.0f;
    public int bulletDamage = 1;
}
