using UnityEngine;

public class Cell
{
    public int gridX;
    public int gridY;
    public bool isObstacle;
    public Vector3 position;
    public Cell parent;

    public int gCost, hCost;

    public int FCost { get { return gCost + hCost; } }

    public Cell (bool IsObstacle, Vector3 Position, int GridX, int GridY)
    {
        isObstacle = IsObstacle;
        position = Position;
        gridX = GridX;
        gridY = GridY;
    }
}
