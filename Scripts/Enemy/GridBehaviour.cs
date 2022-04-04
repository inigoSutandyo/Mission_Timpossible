using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBehaviour : MonoBehaviour
{
    public int rows = 10;
    public int columns = 10;
    public int scale = 1;
    public GameObject gridPrefab;
    public GridNode[,] gridArray;
    public LayerMask wall;
    // Start is called before the first frame update

    private void Awake()
    {
        gridArray = new GridNode[columns, rows];
        GenerateGrid();
    }

    private void GenerateGrid()
    {
         
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GameObject obj = Instantiate(gridPrefab);
                obj.transform.SetParent(gameObject.transform);
                //obj.transform.localPosition = new Vector3(leftBottomLocation.x + scale * i, leftBottomLocation.y, leftBottomLocation.z + scale * j);
                obj.transform.localPosition = new Vector3(scale * i, 0,scale * j);
                obj.transform.localRotation = Quaternion.identity;
                GridNode test = obj.GetComponent<GridNode>();
                test.y = i;
                test.x = j;
                if (Physics.CheckSphere(test.transform.position, 2.0f, wall))
                {
                    test.isObstacle = true;
                } else
                {
                    test.isObstacle = false;
                }
                gridArray[i, j] = test;

            }
        }
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
}
