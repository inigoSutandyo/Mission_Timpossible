using System;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    Grid grid;
    public Transform startPosition;
    public Transform targetPosition;
    private bool createGrid = false;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Update()
    {
        FindPath(startPosition.position, targetPosition.position);
        //float distance = Vector3.Distance(startPosition.position, targetPosition.position);

        //if (distance <= 25.0f)
        //{
        //    grid.CreateGrid();
        //    createGrid = true;
        //}

        //if (createGrid)
        //{
        //    FindPath(startPosition.position, targetPosition.position);
        //}
    }

    private void FindPath(Vector3 startPos, Vector3 endPos)
    {
        Cell startCell = grid.CellFromWorldPosition(startPos);
        Cell targetCell = grid.CellFromWorldPosition(endPos);
        
        print("Start Pos = " + startPos + " > " + "End Pos = " + endPos);
        print("Start Cell = " + startCell.position + " > " + "End Cell = " + targetCell.position);

        List<Cell> OpenList = new List<Cell>();
        HashSet<Cell> CloseList = new HashSet<Cell>();

        OpenList.Add(startCell);

        while (OpenList.Count > 0)
        {
            Cell currCell = OpenList[0];

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
                GetFinalPath(startCell, targetCell);
            }

            foreach (Cell neighbor in grid.GetNeighbors(currCell))
            {
                if(!neighbor.isObstacle || CloseList.Contains(neighbor))
                {
                    continue;
                }

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

    private int GetManhattahnDistance(Cell currCell, Cell neighbor)
    {
        int ix = Mathf.Abs(currCell.gridX - neighbor.gridX);
        int iy = Mathf.Abs(currCell.gridY - neighbor.gridY);

        return ix + iy;
    }

    private void GetFinalPath(Cell startCell, Cell targetCell)
    {
        List<Cell> finalPath = new List<Cell>();

        Cell currCell = targetCell;

        while (currCell != startCell)
        {
            finalPath.Add(currCell);
            currCell = currCell.parent;
        }

        finalPath.Reverse();
        grid.FinalPath = finalPath;
        
    }
}
