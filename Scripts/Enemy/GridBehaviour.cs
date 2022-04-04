using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBehaviour : MonoBehaviour
{
    // maximum grids = 3600
    private int maxGrid = 3600;
    public int rows = 10;
    [HideInInspector] public int columns = 10;
    [HideInInspector] public float scale = 1;
    //public GameObject gridPrefab;
    public GridNode[,] gridArray;
    public LayerMask wall;
    // Start is called before the first frame update

    private void Awake()
    {
        gridArray = new GridNode[columns, rows];
        GenerateGrid();
    }

    public void Start()
    {
        columns = rows;
        scale = (float) Math.Sqrt((float) maxGrid/(rows * columns));
        print(scale);
    }

    private void GenerateGrid()
    {
         
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                //GameObject obj = Instantiate(gridPrefab);
                //obj.transform.SetParent(gameObject.transform);
                //obj.transform.localPosition = new Vector3(leftBottomLocation.x + scale * i, leftBottomLocation.y, leftBottomLocation.z + scale * j);
                //obj.transform.localPosition = new Vector3(scale * i, 0,scale * j);
                //obj.transform.localRotation = Quaternion.identity;
                //GridNode node = obj.GetComponent<GridNode>();

                // Optimized version of above code
                // Doesnt use a game object instead just save position of said object in a class 
                GridNode node = new GridNode();
                node.localPosition = new Vector3(scale * i, 0, scale * j);
                node.position = TransformLocalToWorld(node.localPosition);
                //print(node.position);
                node.y = i;
                node.x = j;
                if (Physics.CheckSphere(node.position, scale, wall))
                {
                    node.isObstacle = true;

                    // Code below visualization unwalkable nodes
                    //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    //cube.transform.position = node.position;
                } else
                {
                    node.isObstacle = false;

                    // Code below visualization walkable nodes
                    //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    //cube.transform.position = node.position;
                }
                gridArray[i, j] = node;

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
}
