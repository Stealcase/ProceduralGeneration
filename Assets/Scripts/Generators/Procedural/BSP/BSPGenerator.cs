﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Stealcase.Generators.Procedural.BSP
{
    public class BSPGenerator
    {

        private int roomWidth, roomHeight;
        RoomNode rootNode;
        /// <summary>
        /// Leafs are the end-points of the tree. 
        /// </summary>
        public List<Node> Leaves = new List<Node>();
        public List<Node> AllNodes = new List<Node>();
        public List<Room> Rooms = new List<Room>();
        public List<Room> Corridors = new List<Room>();
        public int[,] Map;


        public BSPGenerator(int roomWidth, int roomHeight)
        {
            this.roomWidth = roomWidth;
            this.roomHeight = roomHeight;
        }

        public void CalculateRooms(int maxIterations, int minRoomSize, int maxRoomSize)
        {
            var rand = new System.Random();
            //Make -1 here to prevent index out of range exceptions
            Vector2Int origin = new Vector2Int(0, 0);
            Vector2Int farCorner = new Vector2Int(roomWidth - 1, roomHeight - 1);
            rootNode = new RoomNode(new RectInt(origin, farCorner), 0, null, maxIterations, minRoomSize, maxRoomSize, rand);

        }
        public int[,] GenerateMap(int maxIterations, int minRoomSize, int maxRoomSize)
        {
            CalculateRooms(maxIterations, minRoomSize, maxRoomSize);
            rootNode.Traverse(AllNodes, Leaves);
            Map = new int[roomWidth, roomHeight];
            for (int i = 0; i < Leaves.Count; i++)
            {
                // Leaves[i].ConnectRooms(Corridors, Map);
                Leaves[i].Room.ToMap(Map);
                Rooms.Add(Leaves[i].Room);
            }
            return Map;
        }

    }
}