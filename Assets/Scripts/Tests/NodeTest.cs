using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Stealcase.Generators.Procedural.BSP;


namespace Tests
{
    public class NodeTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void TestNode()
        {
            var room = new Room(new Vector2Int(0, 0), new Vector2Int(10, 10), 5);

        }

    }
}
