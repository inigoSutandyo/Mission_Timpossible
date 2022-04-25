using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class VillagePatrol : MonoBehaviour
{
    private EnemyController Enemy;
    public GridBehaviour GridManager;

    private bool arriveAtPoint = false;
    private bool isMovementStarted = false;

    public Transform PatrolPosition1;
    public Transform PatrolPosition2;
    private GridNode currTargetPoint;
    private List<GridNode> PatrolPoint;
    void Awake()
    {
        Enemy = GetComponent<EnemyController>();
        //GridArray = GridManager.GetGridArray();
    }

    private void Start()
    {

        InitPatrol();
    }

    private void InitPatrol()
    {
        //print(GridManager.GetGridArray().Length);
        PatrolPoint = new List<GridNode>
        {
            GridManager.GetGridNode(GridManager.TransformWorldToLocal(PatrolPosition1.position)),
            GridManager.GetGridNode(GridManager.TransformWorldToLocal(PatrolPosition2.position))
        };

        Enemy.isPatrol = true;
        arriveAtPoint = false;
        currTargetPoint = PatrolPoint[0];
        FindNextPatrol();

        

    }

    void Update()
    {

        if (!Enemy.isDetectPlayer && !Enemy.isPatrol)
        {
            Enemy.isPatrol = true;
            FindNextPatrol(); 
        }
        //print(Enemy.isPatrol);
        if (Enemy.isPatrol)
        {
            CheckArriveAtPoint();
            if (arriveAtPoint)
            {
               
                FindNextPatrol();
                arriveAtPoint = false;
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
            arriveAtPoint = true;
        } else
        {
            arriveAtPoint = false;
        }
    }

    private void FindNextPatrol()
    {
        if (currTargetPoint == PatrolPoint[0])
        {
            GoToSecondPatrol();
            currTargetPoint = PatrolPoint[1];
        } else
        {
            GoToFirstPatrol();
            currTargetPoint = PatrolPoint[0];
        }
    }

    private void GoToFirstPatrol()
    {
        GridNode unitNode = GridManager.GetGridNode(GridManager.TransformWorldToLocal(Enemy.transform.position));
        Enemy.movement.StartPathing(unitNode, PatrolPoint[0]);
        arriveAtPoint = false;
    }

    private void GoToSecondPatrol()
    {
        GridNode unitNode = GridManager.GetGridNode(GridManager.TransformWorldToLocal(Enemy.transform.position));
        Enemy.movement.StartPathing(unitNode, PatrolPoint[1]);
        arriveAtPoint = false;
    }
}
