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

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator NewTestScriptWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
