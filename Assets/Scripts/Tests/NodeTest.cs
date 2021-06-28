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
        public void HorizontalSplitLocationSymmetry()
        {
            var rand = new System.Random();
            var rootNode = new Stealcase.Generators.Procedural.BSP.Node(new RectInt(new Vector2Int(0, 0), new Vector2Int(10, 10)),0, null, 0, 3, 5, rand);
            var tup = rootNode.CreateHorizontalSplit();
            var left = tup.Item1;
            var right = tup.Item2;
            Assert.AreEqual(left.yMax, right.yMin);
        }
        [Test]
        public void HorizontalSplitTopMaxInsideRect()
        {
            var rand = new System.Random();
            var rootNode = new Stealcase.Generators.Procedural.BSP.Node(new RectInt(new Vector2Int(0, 0), new Vector2Int(10, 10)),0, null, 0, 3, 5, rand);
            var tup = rootNode.CreateHorizontalSplit();
            var left = tup.Item1;
            var right = tup.Item2;
            Assert.AreEqual(rootNode.Rect.max, right.max);
        }
        [Test]
        public void HorizontalSplitTopMinInsideRect()
        {
            var rand = new System.Random();
            var rootNode = new Stealcase.Generators.Procedural.BSP.Node(new RectInt(new Vector2Int(0, 0), new Vector2Int(10, 10)),0, null, 0, 3, 5, rand);
            var tup = rootNode.CreateHorizontalSplit();
            var left = tup.Item1;
            var right = tup.Item2;
            Assert.IsTrue(rootNode.Rect.Contains(right.min));
        }
        [Test]
        public void HorizontalSplitLeftWallCorrect()
        {
            var rand = new System.Random();
            var rootNode = new Stealcase.Generators.Procedural.BSP.Node(new RectInt(new Vector2Int(0, 0), new Vector2Int(10, 10)),0, null, 0, 3, 5, rand);
            var tup = rootNode.CreateHorizontalSplit();
            var left = tup.Item1;
            var right = tup.Item2;
            Assert.AreEqual(left.xMin, rootNode.Rect.xMin);
        }
        [Test]
        public void HorizontalSplitBottomMinInsideNode()
        {
            var rand = new System.Random();
            var rootNode = new Stealcase.Generators.Procedural.BSP.Node(new RectInt(new Vector2Int(0, 0), new Vector2Int(10, 10)),0, null, 0, 3, 5, rand);
            var tup = rootNode.CreateHorizontalSplit();
            var left = tup.Item1;
            var right = tup.Item2;
            Assert.IsTrue(rootNode.Rect.Contains(left.min));
        }
        [Test]
        public void HorizontalSplitBottomXMaxInsideRect()
        {
            var rand = new System.Random();
            var rootNode = new Stealcase.Generators.Procedural.BSP.Node(new RectInt(new Vector2Int(0, 0), new Vector2Int(10, 10)),0, null, 0, 3, 5, rand);
            var tup = rootNode.CreateHorizontalSplit();
            var left = tup.Item1;
            var right = tup.Item2;
            Assert.AreEqual(rootNode.Rect.xMax, left.xMax);
        }
    }
}
