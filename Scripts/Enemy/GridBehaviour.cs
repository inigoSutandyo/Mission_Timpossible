using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBehaviour : MonoBehaviour
{
    public int fixRow = 60;
    public int fixColumn = 60;
    public int rows = 60;
    public int columns = 60;
    [HideInInspector] public float scale = 1;
    //public GameObject gridPrefab;
    private GridNode[,] gridArray;
    public LayerMask wall;
    // Start is called before the first frame update
    public bool isSafeGrid = false;
    private int maxGrid;

    private void Awake()
    {
        gridArray = new GridNode[columns, rows];
    }

    public void Start()
    {
        maxGrid = fixRow * fixColumn;
        scale = (float) Math.Sqrt((float) maxGrid/(rows * columns));
        GenerateGrid();
        isSafeGrid = true;
        print(scale);
    }

    private void GenerateGrid()
    {
         
        for (float i = 0; i < columns; i++)
        {
            for (float j = 0; j < rows; j++)
            {
                //print(scale * i + " " + scale * j);
                GridNode node = new GridNode
                {
                    localPosition = new Vector3(scale * i, 0, scale * j)
                };
                node.position = TransformLocalToWorld(node.localPosition);
                //print(node.position);
                node.y = (int) i;
                node.x = (int) j;
                node.isPatrolPoint = false;
                if (Physics.CheckSphere(node.position, 1, wall))
                {
                    node.isObstacle = true;

                    // Code below visualization unwalkable nodes
                    //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    //cube.transform.position = node.position;
                }
                else
                {
                    node.isObstacle = false;

                    // Code below visualization walkable nodes
                    //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    //cube.transform.position = node.position;
                }
                gridArray[(int) i, (int) j] = node;
                //print(i + " , " + j + " : " + node);
            }
        }
    }

    private Vector3 TransformLocalToWorld(Vector3 toTransform)
    {
        Vector3 posDiff = gameObject.transform.position - gameObject.transform.localPosition;
        toTransform += posDiff;
        return toTransform + gameObject.transform.localPosition;
    }

    public Vector3 TransformWorldToLocal(Vector3 toTransform)
    {
        Vector3 posDiff = gameObject.transform.position - gameObject.transform.localPosition;

        return TransformToNodePosition(toTransform - posDiff);
    }

    public Vector3 TransformToNodePosition(Vector3 toTransform)
    {
        return toTransform - gameObject.transform.localPosition;
    }

    public GridNode[,] GetGridArray()
    {
        return gridArray;
    }

    public void SetNodeAsObstacle(int x, int y)
    {
        gridArray[y, x].isObstacle = true;
    }

    public void SetNodeAsWalkable(int x, int y)
    {
        gridArray[y, x].isObstacle = false;
    }

    public GridNode GetGridNode(Vector3 pos)
    {
        //print(pos.x + " ## " + pos.y);
        int pX = (int)(pos.x / scale);
        int pY = (int)(pos.z / scale);
        //print(pX + " <> " + pY);
        if (pX < 0 || pX >= columns || pY < 0 || pY >= rows)
        {
            return null;
        }
        return gridArray[pX, pY];
    }
}
