using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCreator : MonoBehaviour
{
    // Start is called before the first frame update

    public int mazeWidth, mazeLength;
    public int roomWidthMin, roomLengthMin;
    public int maxIterate;
    public int corridorWidth;
    public Material material;

    [Range(0.0f, 0.3f)]
    public float roomBottomCornerModifier;
    [Range(0.7f, 1f)]
    public float roomTopCornerModifier;
    [Range(0, 2)]
    public int roomOffset;
    void Start()
    {
        CreateMaze();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateMaze()
    {
        MazeGenerator generator = new MazeGenerator(mazeWidth, mazeLength);
        var listOfRooms = generator.CalculateMaze(
            maxIterate, 
            roomWidthMin, 
            roomLengthMin, 
            roomBottomCornerModifier, 
            roomTopCornerModifier, 
            roomOffset,
            corridorWidth
            );

        for (int i = 0; i < listOfRooms.Count; i++)
        {
            CreateMesh(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner);
        }
    }

    private void CreateMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner)
    {
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, 0, bottomLeftCorner.y);

        Vector3 bottomRightV = new Vector3(topRightCorner.x, 0, bottomLeftCorner.y);

        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0, topRightCorner.y);

        Vector3 topRightV = new Vector3(topRightCorner.x, 0, topRightCorner.y);

        Vector3[] vertices = new Vector3[]
        {
            topLeftV,
            topRightV,
            bottomLeftV,
            bottomRightV
        };

        Vector2[] uvs = new Vector2[vertices.Length];

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] triangles = new int[]
        {
            0,1,2,2,1,3
        };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        GameObject mazeFloor = new GameObject("Mesh" + bottomLeftCorner, typeof(MeshFilter), typeof (MeshRenderer));

        mazeFloor.transform.position = Vector3.zero;
        mazeFloor.transform.localScale = Vector3.one;
        mazeFloor.GetComponent<MeshFilter>().mesh = mesh;
        mazeFloor.GetComponent<MeshRenderer>().material = material;

    }
}
