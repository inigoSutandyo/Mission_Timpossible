using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public bool objectFound = false;
    //private bool isTraversing = false;
    private Vector3[] FinalPath;
    [SerializeField] private GridBehaviour GridManager;

    //public GridNode[,] gridArray;

    private int m_index = 0;
    public bool arriveAtNode = false;
    private CharacterController characterController;

    void Awake()
    {
        FinalPath = new Vector3[300];
        //gridArray = GridManager.gridArray;
        characterController = GetComponent<CharacterController>();
    }

    

    public Vector3[] StartPathing(GridNode startCell, GridNode targetCell)
    {
        
        m_index = 0;
        arriveAtNode = true;
        return FindPath(startCell, targetCell);
    }


    public void TraversePathForBoss(BossBehaviour behaviour)
    {
        if (FinalPath.Length <= 0) return;
        //print(arriveAtNode);
        if (arriveAtNode)
        {
            // add new path
            var new_path = FinalPath[m_index];
            // rotate enemy to new path
            Rotate(new_path);
            arriveAtNode = false;

        }

        if (!arriveAtNode)
        {
            Rotate(FinalPath[m_index]);
            var offset = FinalPath[m_index] - transform.position;
            //print(offset.magnitude);
            if (offset.magnitude > .1f)
            {
                offset = offset.normalized * behaviour.movementSpeed;
                characterController.Move(offset * Time.deltaTime);
            }
            else
            {
                arriveAtNode = true;
                m_index += 1;
                if (m_index >= FinalPath.Length)
                {
                    Array.Clear(FinalPath, 0, FinalPath.Length);
                    objectFound = false;
                }
            }
        }
    }

    public void TraversePath(EnemyBehaviour behaviour)
    {
        if (FinalPath.Length <= 0) return;
        //print(arriveAtNode);
        if (arriveAtNode)
        {
            // add new path
            var new_path = FinalPath[m_index];
            // rotate enemy to new path
            Rotate(new_path);
            arriveAtNode = false;

        }
        
        if (!arriveAtNode)
        {
            Rotate(FinalPath[m_index]);
            var offset = FinalPath[m_index] - transform.position;
            //print(offset.magnitude);
            if (offset.magnitude > .1f)
            {
                offset = offset.normalized * behaviour.movementSpeed;
                characterController.Move(offset * Time.deltaTime);
            }
            else
            {
                arriveAtNode = true;
                m_index += 1;
                if (m_index >= FinalPath.Length)
                {
                    Array.Clear(FinalPath, 0, FinalPath.Length);
                    objectFound = false;
                }
            }
        }
    }

    public bool CheckPath()
    {
        if (FinalPath.Length <= 0 || FinalPath == null)
        {
            return false;
        } else
        {
            return true;
        }
    }

    public void ClearPath()
    {
        if (CheckPath())
        {
            Array.Clear(FinalPath, 0, FinalPath.Length);
            objectFound = false;
        }
    }

    public void Rotate(Vector3 targetPosition)
    {
        //print("Rotating : " + targetPosition);
        var lookPos = targetPosition - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10f);

    }

    public Vector3[] FindPath(GridNode startCell, GridNode targetCell)
    {
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

            if (currCell == null)
            {
                break;
            }

            OpenList.Remove(currCell);

            CloseList.Add(currCell);
            
            if (currCell == targetCell)
            {
                objectFound = true;
                //print("Player Found = " + targetCell.x + "," + targetCell.y);
                FinalPath = GetFinalPath(startCell, targetCell);
                //VisualizeFinalPath();
                return FinalPath;
            }

            

            foreach (GridNode neighbor in GetNeighbors(currCell))
            {
                if (neighbor == null) continue;
                
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
        return null;
    }

    private void VisualizeFinalPath()
    {
        foreach (var path in FinalPath)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.transform.position = path;
        }
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
                var neighbor = GridManager.GetGridArray()[yCheck, xCheck];
                if (neighbor != null)
                    neighborList.Add(neighbor);
            }
        }

        // Left
        xCheck = cell.x - 1;
        yCheck = cell.y;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                var neighbor = GridManager.GetGridArray()[yCheck, xCheck];
                if (neighbor != null)
                    neighborList.Add(neighbor);
            }
        }

        // Top
        xCheck = cell.x;
        yCheck = cell.y + 1;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                var neighbor = GridManager.GetGridArray()[yCheck, xCheck];
                if (neighbor != null)
                    neighborList.Add(neighbor);
            }
        }

        // Bottom
        xCheck = cell.x;
        yCheck = cell.y - 1;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                var neighbor = GridManager.GetGridArray()[yCheck, xCheck];
                if (neighbor != null)
                    neighborList.Add(neighbor);
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

    private /*List<GridNode>*/Vector3[] GetFinalPath(GridNode startCell, GridNode targetCell)
    {
        //print(startCell.position + " --> " + targetCell.position);
        List<GridNode> finalPath = new List<GridNode>();

        GridNode currCell = targetCell;

        while (currCell != startCell)
        {
            
            finalPath.Add(currCell);
            currCell = currCell.parent;
        }
        Vector3[] simplePath = SimplifyPath(finalPath);
        //print("Found");
        //finalPath.Reverse();
        Array.Reverse(simplePath);
        return simplePath;
        
    }

    private Vector3[] SimplifyPath(List<GridNode> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            // Direction between two nodes
            Vector2 directionNew = new Vector2(path[i - 1].position.x - path[i].position.x, path[i - 1].position.y - path[i].position.y);

            //// If path has changed direction
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].position + Vector3.zero);
            }
            directionOld = directionNew;
            
        }
        return waypoints.ToArray();
    }
}
