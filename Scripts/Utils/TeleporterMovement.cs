using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterMovement : MonoBehaviour
{
    private EnemyController Enemy;
    public GridBehaviour GridManager;

    private bool arriveAtPoint = false;

    private GridNode currTargetPoint;
    void Awake()
    {
        Enemy = GetComponent<EnemyController>();
    }


    private void Start()
    {
        StartCoroutine(EnemyMoveTrigger());
        StartCoroutine(EnemyMoveReset());
    }

    void Update()
    {
        //if (Enemy.isPatrol)
        //{
        //    CheckArriveAtPoint();
        //    if (arriveAtPoint)
        //    {
        //        //print("Go Next");
        //        GoToNextPatrol();
        //    }
        //}
    }

    private IEnumerator EnemyMoveTrigger()
    {
        
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (!Enemy.isPatrol)
            {
                if (!Enemy.isDetectPlayer)
                {
                    print("Start Patrol");
                    Enemy.isPatrol = true;
                    GoToNextPatrol();
                }
                continue;
            } else
            {

                CheckArriveAtPoint();
                if (arriveAtPoint)
                {
                    GoToNextPatrol();
                }
            }  
        }
    }

    private IEnumerator EnemyMoveReset()
    {
        while (true)
        {
            yield return new WaitForSeconds(.5f);
            if (!Enemy.isPatrol && !Enemy.isDetectPlayer)
            {
                continue;
            }
            var position = transform.position;
            yield return new WaitForSeconds(1f);
            var new_position = transform.position;

            if (position == new_position)
            {
                Enemy.movement.ClearPath();
                Enemy.isPatrol = false;
                Enemy.isDetectPlayer = false;
            }
        }
    }

    private void CheckArriveAtPoint()
    {
        var dist = Enemy.transform.position - currTargetPoint.position;
        //print(dist.magnitude);
        if (dist.magnitude <= 1.2f)
        {
            //print("arrived");
            currTargetPoint.isPatrolPoint = false;
            arriveAtPoint = true;
        }
        else
        {
            arriveAtPoint = false;
        }
    }

    private void GoToNextPatrol()
    {
        while(true)
        {
            GridNode unitNode = GridManager.GetGridNode(GridManager.TransformWorldToLocal(Enemy.transform.position));
            currTargetPoint = GetRandomPoint(unitNode);

            if (Enemy.movement.StartPathing(unitNode, currTargetPoint) == null)
            {
                Enemy.movement.ClearPath();
            } else
            {
                arriveAtPoint = false;
                break;
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
            } else
            {
                patrolNode.isPatrolPoint = true;
                return patrolNode;
            }
        }
    }
}
