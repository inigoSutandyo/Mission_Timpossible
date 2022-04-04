using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyBehaviour behaviour;
    private bool playerFound = false;
    //private bool isTraversing = false;
    private List<GridNode> FinalPath;
    public GridBehaviour GridManager;


    public Transform playerPosition;
    public GridNode[,] gridArray;

    private float time = 0f;
    private float timeDelay = 0.2f;

    [SerializeField] private PlayerStatus playerStatus;

    public Animator enemyController;
    void Awake()
    {
        behaviour = GetComponent<EnemyBehaviour>();
        gridArray = GridManager.gridArray;
    }

    private void Start()
    {
        FinalPath = new List<GridNode>();
        //StartCoroutine(TraversePath());
        
    }

    // Update is called once per frame
    void Update()
    {
        time += 1f* Time.deltaTime;

        float distance = Vector3.Distance(transform.position, playerPosition.position);
        
        if (distance <= behaviour.noticeRadius)
        {
            if (distance <= behaviour.attackRadius)
            {
                print("Pew Pew");
                Rotate(playerPosition.position);
                if (time >= timeDelay)
                {
                    playerStatus.health -= 1;
                    time = 0;
                }
                playerFound = false;
                FinalPath.Clear();
            } else
            {
                print("Finding path..." + playerFound + " >> " + FinalPath.Count);
                GridNode playerNode = GetGridNode(GridManager.TransformWorldToLocal(playerPosition.position));
                GridNode startNode = GetGridNode(GridManager.TransformWorldToLocal(transform.position));
                if (playerNode != null && !playerFound)
                {
                    
                    //print("Player X = " + playerNode.x);
                    //print("Player = " + playerNode.x + "," + playerNode.y);
                    FindPath(startNode, playerNode);
                }
                else
                {
                    enemyController.SetBool("isRunning", false);
                }


                if (playerFound && FinalPath.Count > 0)
                {
                    TraversePath();
                    enemyController.SetBool("isRunning", true);
                    transform.position += transform.forward * Time.deltaTime * behaviour.movementSpeed;
                }
            }
            
            

            
        } else
        {
            playerFound = false;
        }

        
    }

    private void TraversePath()
    {
        foreach (var path in FinalPath)
        {
            Rotate(path.position);

        }
        FinalPath.Clear();
        playerFound = false;

        //if (FinalPath.Count > 0 && playerFound)
        //{
        //    //Rotate(FinalPath[0].position);
        //    foreach (var path in FinalPath)
        //    {
        //        //Rotate(path.gameObject.transform.position);
        //        Rotate(path.position);
        //        //yield return new WaitForSeconds(0.2f);
        //    }
        //    FinalPath.Clear();
        //    playerFound = false;
        //}

        //int count = FinalPath.Count;
        //int i = 0;
        //bool rotate = false;
        //while (i < count)
        //{
        //    if (!rotate)
        //    {
        //        Rotate(FinalPath[0].position);
        //        rotate = true;
        //    }

        //    if (time >= timeDelay)
        //    {
        //        rotate = false;
        //        time = 0;
        //        i += 1;
        //    }
        //}
        //playerFound = false;
        //isTraversing = false;
    }

    private void Rotate(Vector3 targetPosition)
    {
        //print("Rotating : " + targetPosition);
        var lookPos = targetPosition - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);
    }

    private void FindPath(GridNode startPos, GridNode endPos)
    {
        GridNode startCell = startPos;
        
        GridNode targetCell = endPos;    
        List<GridNode> OpenList = new List<GridNode>();
        HashSet<GridNode> CloseList = new HashSet<GridNode>();
        
        OpenList.Add(startCell);

        while (OpenList.Count > 0)
        {
            GridNode currCell = OpenList[0];

            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].FCost < currCell.FCost || OpenList[i].FCost == currCell.FCost && OpenList[i].hCost < currCell.hCost)
                {
                    currCell = OpenList[i];
                }
            }

            OpenList.Remove(currCell);
            CloseList.Add(currCell);
            
            if (currCell == targetCell)
            {
                playerFound = true;
                print("Player Found = " + targetCell.x + "," + targetCell.y);
                FinalPath = GetFinalPath(startCell, targetCell);
            }

            

            foreach (GridNode neighbor in GetNeighbors(currCell))
            {
                
                if (neighbor.isObstacle || CloseList.Contains(neighbor))
                {
                    continue;
                }
                //print(targetCell.x + " | " + targetCell.y);
                int moveCost = currCell.gCost + GetManhattahnDistance(currCell, neighbor);

                if (moveCost < neighbor.gCost || !OpenList.Contains(neighbor))
                {
                    neighbor.gCost = moveCost;
                    neighbor.hCost = GetManhattahnDistance(neighbor, targetCell);
                    neighbor.parent = currCell;

                    if (!OpenList.Contains(neighbor))
                    {
                        OpenList.Add(neighbor);
                    }
                }
            }
        }
    }

    private GridNode GetGridNode(Vector3 pos)
    {
        //print(pos.x + " ## " + pos.y);
        int pX = (int)(pos.x / GridManager.scale);
        int pY = (int)(pos.z / GridManager.scale);
        //print(pX + " <> " + pY);
        if (pX < 0 || pX >= GridManager.rows || pY < 0 || pY >= GridManager.columns)
        {
            return null;
        }
        return gridArray[pX, pY];
    }

    private List<GridNode> GetNeighbors(GridNode cell)
    {
        List<GridNode> neighborList = new List<GridNode>();
        int xCheck;
        int yCheck;

        int gridSizeX = GridManager.rows;
        int gridSizeY = GridManager.columns;
        if (cell == null)
        {
            print("cell is null");
            return null;
        }
        // Right
        xCheck = cell.x + 1;
        yCheck = cell.y;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighborList.Add(gridArray[xCheck, yCheck]);
            }
        }

        // Left
        xCheck = cell.x - 1;
        yCheck = cell.y;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighborList.Add(gridArray[xCheck, yCheck]);
            }
        }

        // Top
        xCheck = cell.x;
        yCheck = cell.y + 1;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighborList.Add(gridArray[xCheck, yCheck]);
            }
        }

        // Bottom
        xCheck = cell.x;
        yCheck = cell.y - 1;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighborList.Add(gridArray[xCheck, yCheck]);
            }
        }

        return neighborList;
    }

    private int GetManhattahnDistance(GridNode currCell, GridNode neighbor)
    {
        int ix = Mathf.Abs(currCell.x - neighbor.x);
        int iy = Mathf.Abs(currCell.y - neighbor.y);

        return ix + iy;
    }

    private List<GridNode> GetFinalPath(GridNode startCell, GridNode targetCell)
    {
        //print(startCell.position + " --> " + targetCell.position);
        List<GridNode> finalPath = new List<GridNode>();

        GridNode currCell = targetCell;

        while (currCell != startCell)
        {
            
            finalPath.Add(currCell);
            currCell = currCell.parent;
        }
        //print("Found");
        finalPath.Reverse();
        return finalPath;
        
    }


}
