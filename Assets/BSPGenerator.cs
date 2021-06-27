using Assets;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BSPGenerator
{

    private int roomWidth, roomHeight;
    RoomNode rootNode;
    /// <summary>
    /// Leafs are the end-points of the tree. 
    /// </summary>
    public List<Node> Leafs = new List<Node>();
    public List<Room> Rooms = new List<Room>();




    public BSPGenerator(int roomWidth, int roomHeight)
    {
        this.roomWidth = roomWidth;
        this.roomHeight = roomHeight;
    }

    public void CalculateRooms(int maxIterations, int minRoomSize, int maxRoomSize)
    {
        var rand = new System.Random();

        rootNode = new RoomNode(new Vector2Int(0, 0), new Vector2Int(roomWidth, roomHeight), 0, null, maxIterations, minRoomSize, maxRoomSize, rand);
        rootNode.Traverse(Leafs, Rooms);
    }

}