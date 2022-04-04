using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    public Transform startPosition;
    public LayerMask obstacle;
    public Vector2 gridWorldSize;
    public float cellRadius, cellDistance;

    Cell[,] grid;
    public List<Cell> FinalPath;

    float cellDiameter;
    int gridSizeX, gridSizeY;
    void Start()
    {
        cellDiameter = cellRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / cellDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / cellDiameter);
        CreateGrid();

    }

    private void CreateGrid()
    {
        grid = new Cell[gridSizeX, gridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * cellDiameter + cellRadius) + Vector3.forward * (y * cellDiameter + cellRadius);

                bool wall = true;

                if (Physics.CheckSphere(worldPoint, cellRadius, obstacle))
                {
                    wall = false;
                }

                grid[x, y] = new Cell(wall, worldPoint, x, y);
            }
        }
    }

    public Cell CellFromWorldPosition(Vector3 startPos)
    {
        float xpoint = ((startPos.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float ypoint = ((startPos.z + gridWorldSize.y / 2) / gridWorldSize.y);

        xpoint = Mathf.Clamp01(xpoint);
        ypoint = Mathf.Clamp01(ypoint);

        int x = Mathf.RoundToInt((gridSizeX - 1) * xpoint);
        int y = Mathf.RoundToInt((gridSizeY - 1) * ypoint);
        print(x + "," + y);
        return grid[x, y];
    }

    public List<Cell> GetNeighbors(Cell cell)
    {
        List<Cell> neighborList = new List<Cell>();
        int xCheck;
        int yCheck;

        // Right
        xCheck = cell.gridX + 1;
        yCheck = cell.gridY;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighborList.Add(grid[xCheck, yCheck]);
            }
        }

        // Left
        xCheck = cell.gridX - 1;
        yCheck = cell.gridY;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighborList.Add(grid[xCheck, yCheck]);
            }
        }

        // Top
        xCheck = cell.gridX;
        yCheck = cell.gridY + 1;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighborList.Add(grid[xCheck, yCheck]);
            }
        }

        // Bottom
        xCheck = cell.gridX;
        yCheck = cell.gridY - 1;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighborList.Add(grid[xCheck, yCheck]);
            }
        }

        return neighborList;
    }
}
