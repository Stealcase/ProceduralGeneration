using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Stealcase.Helpers;

namespace Tests
{
    public class TestNumberGenerator
    {
        // A Test behaves as an ordinary method
        [Test]
        public void TestNumberBetweenNumber()
        {
            // Use the Assert class to test conditions
            var num = VectorHelper.NumberBetweenNumbers(5, 12, 2);
            Assert.Greater(num,6);
            Assert.Less(num,11);
        }
        [Test]
        public void TestRectWithinBoundsWidth()
        {
            var origin = new Vector2Int(0, 0);
            var size = new Vector2Int(10, 10);
            RectInt rect = VectorHelper.RectWithinBounds(origin, size, 2);

            Assert.IsTrue(rect.width >= 2);
        }
        [Test]
        public void TestRectWithinBoundsHeight()
        {
            var origin = new Vector2Int(0, 0);
            var size = new Vector2Int(10, 10);
            RectInt rect = VectorHelper.RectWithinBounds(origin, size, 2);

            Assert.IsTrue(rect.height >= 2);
        }
        [Test]
        public void TestRectWithinBoundsWithinBoundsLeft()
        {
            var origin = new Vector2Int(0, 0);
            var size = new Vector2Int(10, 10);
            var container = new RectInt(origin, size);
            RectInt rect = VectorHelper.RectWithinBounds(origin, size, 2);

            Assert.IsTrue(container.Contains(rect.min));
        }
        [Test]
        public void TestRectWithinBoundsWithinBoundsRight()
        {
            var origin = new Vector2Int(0, 0);
            var size = new Vector2Int(10, 10);
            var container = new RectInt(origin, size);
            RectInt rect = VectorHelper.RectWithinBounds(origin, size, 2);

            Assert.IsTrue(container.Contains(rect.max));
        }

        // // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // // `yield return null;` to skip a frame.
        // [UnityTest]
        // public IEnumerator NewTestScriptWithEnumeratorPasses()
        // {
        //     // Use the Assert class to test conditions.
        //     // Use yield to skip a frame.
        //     yield return null;
        // }
    }
}
