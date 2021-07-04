using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Stealcase.Generators.Procedural.BSP;
using Stealcase.Generators.Procedural;
using System;

namespace Tests
{
    public class RoomTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void TestRoomBottomLeft()
        {
            var origin = new Vector2Int(0, 0);
            var size = new Vector2Int(10, 10);
            RectInt rect = new RectInt(origin, size);
            var room = new Room(origin, size, 5);
            Assert.IsTrue(rect.Contains(room.BottomLeft));
        }
        [Test]
        public void TestRoomTopRight()
        {
            var origin = new Vector2Int(0, 0);
            var size = new Vector2Int(10, 10);
            RectInt rect = new RectInt(origin, size);
            var room = new Room(origin, size, 5);
            Assert.IsTrue(rect.Contains(room.TopRight));
        }
        [Test]
        public void TestRoomToMapDontThrow()
        {
            var expected = 64;
            var expected2 = 128;
            var minimumRoomSize = 5;
            //Generate array where every coordinate is "1"
            var arr = MapArrayGenerator.GenerateArray(expected,expected2, false);
            var origin = new Vector2Int(0, 0);
            var size = new Vector2Int(64, 64);
            for (int i = 0; i < 100; i++)
            {
                var room = new Room(origin, size, minimumRoomSize);
                Assert.DoesNotThrow(()=> room.ToMap(arr),"message");
            }
        }
        [Test]
        public void RoomMappedToMap()
        {
            var expected = 64;
            var expected2 = 128;
            var minimumRoomSize = expected;
            var nodeOrigin = new Vector2Int(0, 0);
            var nodeSize = new Vector2Int(expected, expected);
            System.Random rand = new System.Random();
            for (int i = 0; i < 100; i++)
            {
                //Generate array where every coordinate is "0"
                var arr = MapArrayGenerator.GenerateArray(expected,expected2, true);
                var room = new Room(nodeOrigin, nodeSize, minimumRoomSize/2, 0,rand);
                room.ToMap(arr);
                Assert.IsTrue(ConfirmMapping(arr,room));
            }
        }
        //Confirm that mapping of room matches 1:1 with Map
        public bool ConfirmMapping(int[,] map, Room room)
        {
            var xStart = room.BottomLeft.x;
            var xEnd = room.TopRight.x;
            var ystart = room.BottomLeft.y;
            var yEnd = room.TopRight.y;
            var mapX = map.GetLength(0);
            var mapY = map.GetLength(1);

            for (int x = xStart; x < xEnd; x++)
            {
                if(mapX <= x)
                {
                    Debug.LogWarning($"X Index out of range: x: {x}");
                    continue;
                }
                for (int y = ystart; y < yEnd; y++)
                {
                    if(mapY <= y)
                    {
                        Debug.LogWarning($"Y Index out of range: x: {x}, y: {y}"); 
                        break;
                    }
                    if(map[x, y] == 1)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
