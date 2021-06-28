using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Stealcase.Generators.Procedural.BSP;


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
        public void TestRoomWidth()
        {
            var origin = new Vector2Int(0, 0);
            var size = new Vector2Int(10, 10);
            RectInt rect = new RectInt(origin, size);
            var room = new Room(origin, size, 5);
            Assert.IsTrue(room.Width >= 5);
        }
        [Test]
        public void TestRoomHeight()
        {
            var origin = new Vector2Int(0, 0);
            var size = new Vector2Int(10, 10);
            RectInt rect = new RectInt(origin, size);
            var room = new Room(origin, size, 5);
            Assert.IsTrue(room.Height >= 5);
        }


    }
}
