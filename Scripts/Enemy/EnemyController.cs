using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [HideInInspector] public EnemyMovement movement;
    [HideInInspector] public EnemyShooting shooter;
    [HideInInspector] public EnemyBehaviour behaviour;

    public GridBehaviour GridManager;

    public Transform playerPosition;

    [SerializeField] private PlayerStatus playerStatus;

    public Animator anim;

    [HideInInspector] public bool isPatrol { get; set; }

    [HideInInspector]
    public bool isDetectPlayer = false;
    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        shooter = GetComponent<EnemyShooting>();
        behaviour = GetComponent<EnemyBehaviour>();
    }

    void Update()
    {
        
        if (shooter.CheckBullet())
        {
            shooter.UpdateBullet(Time.deltaTime);
        }

        if (CheckDistance())
        {
            isDetectPlayer = true;
            if (isPatrol)
            {
                movement.ClearPath();
                movement.objectFound = false;
            }
            isPatrol = false;
            if (CheckAttack())
            {
                movement.objectFound = false;
                movement.ClearPath();

                StartRun(false);
                StartShoot(true);
                movement.Rotate(playerPosition.position);
                if (!shooter.isShooting)
                {
                    shooter.Shoot();
                } else if (shooter.isShooting)
                {
                    shooter.UpdateShoot(Time.deltaTime);
                }
            }
            
            if(!CheckAttack())
            {
                GridNode playerNode = GridManager.GetGridNode(GridManager.TransformWorldToLocal(playerPosition.position));
                GridNode startNode = GridManager.GetGridNode(GridManager.TransformWorldToLocal(transform.position));
                StartShoot(false);
                if (playerNode != null && !movement.objectFound)
                {
                    //print("Finding Player");
                    movement.StartPathing(startNode, playerNode);
                }          
            }
        }
        
        if (!CheckDistance())
        {
            isDetectPlayer = false;
            if (!isPatrol)
            {
                movement.ClearPath();
            }
        }

        if (movement.objectFound && movement.CheckPath())
        {
            StartRun(true);
            movement.TraversePath(behaviour);
        }
    }

    private bool CheckDistance()
    {
        float distance = Vector3.Distance(transform.position, playerPosition.position);
        return distance <= behaviour.noticeRadius;
    }

    private bool CheckAttack()
    {
        float distance = Vector3.Distance(transform.position, playerPosition.position);
        return distance <= behaviour.attackRadius;
    }


    public void StartRun(bool action)
    {
        bool running = anim.GetBool("isRunning");
        if (running != action)
        {
            anim.SetBool("isRunning", action);
        }
    }

    public void StartShoot(bool action)
    {
        bool shooting = anim.GetBool("isShooting");
        if (shooting != action)
        {
            anim.SetBool("isShooting", action);
        }
    }



}
