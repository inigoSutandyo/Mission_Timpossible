using System;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator
{
    
    List<RoomNode> allNodesCollection = new List<RoomNode>();
    private int mazeWidth;
    private int mazeLength;

    public MazeGenerator(int mazeWidth, int mazeLength)
    {
        this.mazeWidth = mazeWidth;
        this.mazeLength = mazeLength;
    }

    internal List<Node> CalculateMaze(
        int maxIterate, 
        int roomWidthMin, 
        int roomLengthMin, 
        float roomBottomCornerModifier, 
        float roomTopCornerModifier, 
        int roomOffset, 
        int corridorWidth)
    {
        BinarySpacePartitioner bsp = new BinarySpacePartitioner(mazeWidth, mazeLength);
        allNodesCollection = bsp.PrepareNodesCollection(maxIterate, roomWidthMin, roomLengthMin);
        List<Node> roomSpaces = StructureHelper.TraverseGraphToExtractLowerLeaves(bsp.RootNode);

        RoomGenerator roomGenerator = new RoomGenerator(maxIterate, roomLengthMin, roomWidthMin);

        List<RoomNode> roomList = roomGenerator.GenerateRoomInAGivenSpaces(roomSpaces, roomBottomCornerModifier, roomTopCornerModifier, roomOffset);

        CorridorsGenerator corridorsGenerator = new CorridorsGenerator();
        var corridorList = corridorsGenerator.CreateCorridor(allNodesCollection, corridorWidth);


        return new List<Node>(roomList);
    }
}