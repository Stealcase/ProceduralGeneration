using System;
using System.Collections.Generic;
using Stealcase.Generators.Procedural.BSP;
using UnityEngine;

namespace Stealcase.Generators.Procedural
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
        public List<Stealcase.Generators.Procedural.BSP.Room> Rooms = new List<BSP.Room>();
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
            var origin = new Vector2Int(0, 0);
            var size = new Vector2Int(roomWidth - 1, roomHeight - 1);
            var rect = new RectInt(origin, size);
            rootNode = new RoomNode(rect, 0, null, maxIterations, minRoomSize, maxRoomSize, rand);
            Debug.Log($"ORIGING SIZE {rect}");
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