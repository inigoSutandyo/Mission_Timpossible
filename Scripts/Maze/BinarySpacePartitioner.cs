using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class BinarySpacePartitioner
{

    RoomNode rootNode;
    public RoomNode RootNode { get => rootNode;}

    public BinarySpacePartitioner(int mazeWidth, int mazeLength)
    {
        this.rootNode = new RoomNode(new Vector2Int(0,0), new Vector2Int(mazeWidth , mazeLength), null, 0);
    }

    public List<RoomNode> PrepareNodesCollection(int maxIterate, int roomWidthMin, int roomLengthMin)
    {
        Queue<RoomNode> graph = new Queue<RoomNode>();

        List<RoomNode> listRoomNode = new List<RoomNode>();
        graph.Enqueue(this.rootNode);
        listRoomNode.Add(this.rootNode);
        int iterate = 0;

        while (iterate < maxIterate && graph.Count > 0)
        {
            iterate += 1;
            RoomNode currNode = graph.Dequeue();

            if (currNode.Width >= roomWidthMin * 2 || currNode.Length >= roomLengthMin * 2)
            {
                SplitSpace(currNode, listRoomNode, roomLengthMin, roomWidthMin, graph);
            }
        }
        return listRoomNode;
    }

    private void SplitSpace(RoomNode currNode, List<RoomNode> listRoomNode, int roomLengthMin, int roomWidthMin, Queue<RoomNode> graph)
    {
        Line line = GetLineDividingSpace(currNode.BottomLeftAreaCorner, currNode.TopRightAreaCorner, roomWidthMin, roomLengthMin);
        RoomNode node1, node2;

        if(line.Orientation == Orientation.Horizontal)
        {
            node1 = new RoomNode(currNode.BottomLeftAreaCorner
                , new Vector2Int(currNode.TopRightAreaCorner.x, line.Coords.y)
                , currNode
                , currNode.TreeLayerIndex + 1);

            node2 = new RoomNode(new Vector2Int(currNode.BottomLeftAreaCorner.x, line.Coords.y)
                , currNode.TopRightAreaCorner
                , currNode
                , currNode.TreeLayerIndex + 1);
        } else
        {
            node1 = new RoomNode(currNode.BottomLeftAreaCorner
                , new Vector2Int(line.Coords.x, currNode.TopRightAreaCorner.y)
                , currNode
                , currNode.TreeLayerIndex + 1);

            node2 = new RoomNode(new Vector2Int(line.Coords.x, currNode.BottomLeftAreaCorner.y)
                , currNode.TopRightAreaCorner
                , currNode
                , currNode.TreeLayerIndex + 1);
        }
        AddNewNodeToCollections(listRoomNode, graph, node1);
        AddNewNodeToCollections(listRoomNode, graph, node2);
    }

    private void AddNewNodeToCollections(List<RoomNode> listRoomNode, Queue<RoomNode> graph, RoomNode node)
    {
        listRoomNode.Add(node);
        graph.Enqueue(node);
    }

    private Line GetLineDividingSpace(Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, int roomWidthMin, int roomLengthMin)
    {
        Orientation orientation;
        bool lengthStatus = (topRightAreaCorner.y - bottomLeftAreaCorner.y) >= 2 * roomWidthMin;
        bool widthStatus = (topRightAreaCorner.x - bottomLeftAreaCorner.x) >= 2 * roomLengthMin;

        if (lengthStatus && widthStatus)
        {
            orientation = (Orientation)(Random.Range(0, 2));
        } else if(widthStatus)
        {
            orientation = Orientation.Vertical;
        } else
        {
            orientation = Orientation.Horizontal;
        }

        return new Line(orientation, GetCoordinatesForOrientation(orientation, 
            bottomLeftAreaCorner, 
            topRightAreaCorner, 
            roomWidthMin, 
            roomLengthMin));
    }

    private Vector2Int GetCoordinatesForOrientation(Orientation orientation, Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, int roomWidthMin, int roomLengthMin)
    {
        Vector2Int coordinates = Vector2Int.zero;

        if (orientation == Orientation.Horizontal)
        {
            coordinates = new Vector2Int(0,
                Random.Range(
                    (bottomLeftAreaCorner.y + roomLengthMin), 
                    (topRightAreaCorner.y - roomLengthMin)
                )
            );
        } else
        {
            coordinates = new Vector2Int(Random.Range(
                    (bottomLeftAreaCorner.x + roomWidthMin),
                    (topRightAreaCorner.x - roomWidthMin)
                ),0);
        }

        return coordinates;
    }
}