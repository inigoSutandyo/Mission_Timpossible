using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode : MonoBehaviour
{
    public int x = 0, y = 0;
    public int gCost, hCost;
    public int FCost { get { return gCost + hCost; } }
    public GridNode parent;
    public bool isObstacle = false;
}
