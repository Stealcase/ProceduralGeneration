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
            var rootNode = new Stealcase.Generators.Procedural.BSP.Node(new RectInt(new Vector2Int(0, 0), new Vector2Int(10, 10)),0, null, 0, 3, 5, 2, 0, rand);
            var tup = rootNode.SplitHorizontally();
            var left = tup.Item1;
            var right = tup.Item2;
            Assert.AreEqual(left.yMax, right.yMin);
        }
        [Test]
        public void HorizontalSplitTopMaxInsideRect()
        {
            var rand = new System.Random();
            var rootNode = new Stealcase.Generators.Procedural.BSP.Node(new RectInt(new Vector2Int(0, 0), new Vector2Int(10, 10)),0, null, 0, 3, 5, 2, 0, rand);
            var tup = rootNode.SplitHorizontally();
            var left = tup.Item1;
            var right = tup.Item2;
            Assert.AreEqual(rootNode.Rect.max, right.max);
        }
        [Test]
        public void HorizontalSplitTopMinInsideRect()
        {
            var rand = new System.Random();
            var rootNode = new Stealcase.Generators.Procedural.BSP.Node(new RectInt(new Vector2Int(0, 0), new Vector2Int(10, 10)),0, null, 0, 3, 5, 2, 0, rand);
            var tup = rootNode.SplitHorizontally();
            var left = tup.Item1;
            var right = tup.Item2;
            Assert.IsTrue(rootNode.Rect.Contains(right.min));
        }
        [Test]
        public void HorizontalSplitLeftWallCorrect()
        {
            var rand = new System.Random();
            var rootNode = new Stealcase.Generators.Procedural.BSP.Node(new RectInt(new Vector2Int(0, 0), new Vector2Int(10, 10)),0, null, 0, 3, 5, 2, 0, rand);
            var tup = rootNode.SplitHorizontally();
            var left = tup.Item1;
            var right = tup.Item2;
            Assert.AreEqual(left.xMin, rootNode.Rect.xMin);
        }
        [Test]
        public void HorizontalSplitBottomMinInsideNode()
        {
            var rand = new System.Random();
            var rootNode = new Stealcase.Generators.Procedural.BSP.Node(new RectInt(new Vector2Int(0, 0), new Vector2Int(10, 10)),0, null, 0, 3, 5, 2, 0, rand);
            var tup = rootNode.SplitHorizontally();
            var left = tup.Item1;
            var right = tup.Item2;
            Assert.IsTrue(rootNode.Rect.Contains(left.min));
        }
        [Test]
        public void HorizontalSplitBothRoomsBiggerThanMinSize()
        {
            var rand = new System.Random();
            var minSize = 3;
            var rootNode = new Stealcase.Generators.Procedural.BSP.Node(new RectInt(new Vector2Int(0, 0), new Vector2Int(10, 10)),0, null, 0, minSize, 5, 2, 0, rand);
            var tup = rootNode.SplitHorizontally();
            var left = tup.Item1;
            var right = tup.Item2;
            Assert.IsTrue(left.width >= minSize && right.width >= minSize && left.height >= minSize && right.height > minSize);
        }
        //Cant test this way because a split cannot guarentee that they are smaller than max size
        // [Test]
        // public void HorizontalSplitBothRoomsSmallerThanMaxSize()
        // {
        //     var rand = new System.Random();
        //     var minSize = 3;
        //     var maxSize = 14;
        //     var rootNode = new Stealcase.Generators.Procedural.BSP.Node(new RectInt(new Vector2Int(0, 0), new Vector2Int(100, 100)),0, null, 0, minSize, maxSize, rand);
        //     var tup = rootNode.SplitHorizontally();
        //     var left = tup.Item1;
        //     var right = tup.Item2;
        //     Assert.IsTrue(left.width >= minSize && right.width >= minSize && left.height >= minSize && right.height > minSize);
        // }
        [Test]
        public void HorizontalSplitBottomXMaxInsideRect()
        {
            var rand = new System.Random();
            var rootNode = new Stealcase.Generators.Procedural.BSP.Node(new RectInt(new Vector2Int(0, 0), new Vector2Int(10, 10)),0, null, 0, 3, 5, 2, 0, rand);
            var tup = rootNode.SplitHorizontally();
            var left = tup.Item1;
            var right = tup.Item2;
            Assert.AreEqual(rootNode.Rect.xMax, left.xMax);
        }
        // A Test behaves as an ordinary method
        [Test]
        public void VerticalSplitLocationSymmetry()
        {
            var rand = new System.Random();
            var rootNode = new Stealcase.Generators.Procedural.BSP.Node(new RectInt(new Vector2Int(0, 0), new Vector2Int(10, 10)),0, null, 0, 3, 5, 2, 0, rand);
            var tup = rootNode.SplitVertically();
            var bottom = tup.Item1;
            var top = tup.Item2;
            Assert.AreEqual(bottom.xMax, top.xMin);
        }
        [Test]
        public void VerticalSplitTopMaxInsideRect()
        {
            var rand = new System.Random();
            var rootNode = new Stealcase.Generators.Procedural.BSP.Node(new RectInt(new Vector2Int(0, 0), new Vector2Int(10, 10)),0, null, 0, 3, 5, 2, 0, rand);
            var tup = rootNode.SplitVertically();
            var bottom = tup.Item1;
            var top = tup.Item2;
            Assert.AreEqual(rootNode.Rect.max, top.max);
        }
        [Test]
        public void VerticalSplitTopMinInsideRect()
        {
            var rand = new System.Random();
            var rootNode = new Stealcase.Generators.Procedural.BSP.Node(new RectInt(new Vector2Int(0, 0), new Vector2Int(10, 10)),0, null, 0, 3, 5, 2, 0, rand);
            var tup = rootNode.SplitVertically();
            var bottom = tup.Item1;
            var top = tup.Item2;
            Assert.IsTrue(rootNode.Rect.Contains(top.min));
        }
        [Test]
        public void VerticalSplitLeftWallCorrect()
        {
            var rand = new System.Random();
            var rootNode = new Stealcase.Generators.Procedural.BSP.Node(new RectInt(new Vector2Int(0, 0), new Vector2Int(10, 10)),0, null, 0, 3, 5, 2, 0, rand);
            var tup = rootNode.SplitVertically();
            var bottom = tup.Item1;
            var top = tup.Item2;
            Assert.AreEqual(bottom.xMin, rootNode.Rect.xMin);
        }
        [Test]
        public void VerticalSplitBottomMinInsideNode()
        {
            var rand = new System.Random();
            var rootNode = new Stealcase.Generators.Procedural.BSP.Node(new RectInt(new Vector2Int(0, 0), new Vector2Int(10, 10)),0, null, 0, 3, 5, 2, 0, rand);
            var tup = rootNode.SplitVertically();
            var bottom = tup.Item1;
            var top = tup.Item2;
            Assert.IsTrue(rootNode.Rect.Contains(bottom.min));
        }
        [Test]
        public void VerticalSplitBottomXMaxInsideRect()
        {
            var rand = new System.Random();
            var rootNode = new Stealcase.Generators.Procedural.BSP.Node(new RectInt(new Vector2Int(0, 0), new Vector2Int(10, 10)),0, null, 0, 3, 5, 2, 0, rand);
            var tup = rootNode.SplitVertically();
            var bottom = tup.Item1;
            var top = tup.Item2;
            Assert.AreEqual(rootNode.Rect.xMax, top.xMax);
        }
        [Test]
        public void VerticalSplitBothRoomsBiggerThanMinSize()
        {
            var rand = new System.Random();
            var minSize = 3;
            var rootNode = new Stealcase.Generators.Procedural.BSP.Node(new RectInt(new Vector2Int(0, 0), new Vector2Int(10, 10)),0, null, 0, minSize, 5, 2, 0, rand);
            var tup = rootNode.SplitVertically();
            var bottom = tup.Item1;
            var top = tup.Item2;
            Assert.IsTrue(bottom.width >= minSize && top.width >= minSize && bottom.height >= minSize && top.height > minSize);
        }
    }
}
