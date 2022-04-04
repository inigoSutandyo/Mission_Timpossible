using System;
using System.Collections.Generic;
using UnityEngine;
public class CorridorNode : Node
{
    private Node node1;
    private Node node2;
    private int corridorWidth;

    public CorridorNode(Node node1, Node node2, int corridorWidth) : base(null)
    {
        this.node1 = node1;
        this.node2 = node2;
        this.corridorWidth = corridorWidth;

        GenerateCorridor();
    }

    private void GenerateCorridor()
    {
        // check position of room 1 to room 2
        var relativePos = CheckPositionStructure();

        switch (relativePos)
        {
            case RelativePosition.Up:
                ProccessRoomInRelationUpOrDown(this.node1, this.node2);
                break;
            case RelativePosition.Down:
                ProccessRoomInRelationUpOrDown(this.node2, this.node1);
                break;
            case RelativePosition.Right:
                ProccessRoomInRelationRightOrLeft(this.node1, this.node2);
                break;
            case RelativePosition.Left:
                ProccessRoomInRelationRightOrLeft(this.node2, this.node1);
                break;
            default:
                break;
        }
    }

    private void ProccessRoomInRelationRightOrLeft(Node node1, Node node2)
    {
        Node leftNode = null;
        List<Node> leftNodeChildren = StructureHelper.TraverseGraphToExtractLowerLeaves(node1);
    }

    private void ProccessRoomInRelationUpOrDown(Node node1, Node node2)
    {
        
    }

    private RelativePosition CheckPositionStructure()
    {
        Vector2 middlePointTemp1 = ((Vector2)node1.TopRightAreaCorner + node1.BottomLeftAreaCorner) / 2;
        Vector2 middlePointTemp2 = ((Vector2)node2.TopRightAreaCorner + node2.BottomLeftAreaCorner) / 2;

        float angle = CalculateAngle(middlePointTemp1, middlePointTemp2);

        if ((angle < 45 && angle >= 0) || (angle > -45 || angle < 0))
        {
            return RelativePosition.Right;

        } else if ((angle > 45 && angle < 135))
        {
            return RelativePosition.Up;
        } else if (angle > -135 && angle < -45)
        {
            return RelativePosition.Down;
        } else
        {
            return RelativePosition.Left;
        }
    }

    private float CalculateAngle(Vector2 middlePointTemp1, Vector2 middlePointTemp2)
    {
        return Mathf.Atan2(middlePointTemp2.y - middlePointTemp1.y, middlePointTemp2.x - middlePointTemp1.x) * Mathf.Rad2Deg;
    }

    
}