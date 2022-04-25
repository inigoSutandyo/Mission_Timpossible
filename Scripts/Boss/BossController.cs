using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    // Start is called before the first frame update

    private EnemyMovement movement;
    private BossShooting shooter;
    private BossBehaviour behaviour;

    public GridBehaviour GridManager;
    [SerializeField] Animator anim;
    public Transform playerPosition;

    private bool isPlayerInArea = false;
    private bool isMoving = false;

    private GridNode currTargetPoint;
    [SerializeField] private AudioSource walkAudio;
    private AudioManager audioManager;
    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        shooter = GetComponent<BossShooting>();
        behaviour = GetComponent<BossBehaviour>();

        audioManager = new AudioManager(walkAudio);
    }

    void Start()
    {
        StartCoroutine(BossMoveTrigger());
    }


    // Update is called once per frame
    void Update()
    {
        if (behaviour.isDead) return;
        isPlayerInArea = CheckPlayerInArea();

        if (isPlayerInArea)
        {
            if (!isMoving)
            {
                audioManager.StopAudio();
                // Shoot
                StartShoot(true);
                ShootEnemy();
            }
        }

        shooter.UpdateBullet(Time.deltaTime);

        

        if (isMoving)
        {
            if (CheckArriveAtPoint())
            {
                isMoving = false;
                StartRun(false);
            }
        }

        if (movement.objectFound && movement.CheckPath())
        {
            //StartRun(true);
            //print("Moving to node");
            audioManager.PlayAudioWithLoop();
            movement.TraversePathForBoss(behaviour);
        }
    }

    private IEnumerator BossMoveTrigger()
    {
        while (true)
        {
            yield return new WaitForSeconds(5.0f);
            StartShoot(false);
            isMoving = true;
            MoveToNode();
        }
    }

    private bool CheckArriveAtPoint()
    {

        var dist = transform.position - currTargetPoint.position;
        if (dist.magnitude <= 1.2f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void MoveToNode()
    {
        
        while (true)
        {
            GridNode unitNode = GridManager.GetGridNode(GridManager.TransformWorldToLocal(transform.position));
            currTargetPoint = GetRandomPoint(unitNode);

            //GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //obj.transform.position = currTargetPoint.position;

            if (movement.StartPathing(unitNode, currTargetPoint) != null)
            {
                StartRun(true);
                break;
            }
            else
            {
                movement.ClearPath();
            }
        }
        
    }
    private GridNode GetRandomPoint(GridNode unitNode)
    {
        while (true)
        {
            var newPos = new Vector3(UnityEngine.Random.Range(-20, 20) + unitNode.position.x, unitNode.position.y, UnityEngine.Random.Range(-20, 20) + unitNode.position.z);

            GridNode patrolNode = GridManager.GetGridNode(GridManager.TransformWorldToLocal(newPos));

            if (patrolNode == null) continue;

            if (patrolNode.isObstacle || patrolNode.isPatrolPoint)
            {
                continue;
            }
            else
            {
                patrolNode.isPatrolPoint = true;
                return patrolNode;
            }
        }
    }

    private bool CheckPlayerInArea()
    {
        GridNode playerNode = GridManager.GetGridNode(GridManager.TransformWorldToLocal(playerPosition.position));
        if (playerNode != null)
        {
            return true;
        }

        return false;
    }



    public void StartRun(bool action)
    {
        bool running = anim.GetBool("isRunning");
        if (running != action)
        {
            anim.SetBool("isRunning", action);
        }
    }

    private void ShootEnemy()
    {
        movement.Rotate(playerPosition.position);
   
        if (!shooter.isShooting)
        {
            shooter.Shoot();
        }
        else if (shooter.isShooting)
        {
            shooter.UpdateShoot(Time.deltaTime);
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
