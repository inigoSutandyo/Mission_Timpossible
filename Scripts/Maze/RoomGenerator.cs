using System;
using System.Collections.Generic;
using UnityEngine;
public class RoomGenerator
{
    private int maxIterate;
    private int roomLengthMin;
    private int roomWidthMin;

    public RoomGenerator(int maxIterate, int roomLengthMin, int roomWidthMin)
    {
        this.maxIterate = maxIterate;
        this.roomLengthMin = roomLengthMin;
        this.roomWidthMin = roomWidthMin;
    }

    public List<RoomNode> GenerateRoomInAGivenSpaces(List<Node> roomSpaces, float roomBottomCornerModifier, float roomTopCornerModifier, int roomOffset)
    {
        List<RoomNode> listRoomNode = new List<RoomNode>();

        foreach (var space in roomSpaces)
        {
            Vector2Int newBottomLeftPoint = StructureHelper.GenerateBottomLeftCornerBetween(
                space.BottomLeftAreaCorner, 
                space.TopLeftAreaCorner, 
                roomBottomCornerModifier,
                roomOffset);

            Vector2Int newTopRightPoint = StructureHelper.GenerateTopRightCornerBetween(
                space.BottomLeftAreaCorner, 
                space.TopLeftAreaCorner, 
                roomTopCornerModifier,
                roomOffset);

            space.BottomLeftAreaCorner = newBottomLeftPoint;
            space.TopLeftAreaCorner = newTopRightPoint;
            space.BottomRightAreaCorner = new Vector2Int(newTopRightPoint.x, newBottomLeftPoint.y);
            space.TopLeftAreaCorner = new Vector2Int(newBottomLeftPoint.x, newTopRightPoint.y);

            listRoomNode.Add((RoomNode)space);
        }

        return listRoomNode;
    }
}