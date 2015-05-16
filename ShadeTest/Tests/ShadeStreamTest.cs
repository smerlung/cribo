using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shade.Objects;
using System.Linq;
using System.Text;

namespace ShadeTest.Tests
{
    [TestClass]
    public class ShadeStreamTest
    {
        [TestMethod]
        public void GetSetShadeBytesTest()
        {
            byte[] data = Enumerable.Range(0, 40).Select(n=>(byte)0xff).ToArray();
            byte[] expected = new byte[] { 1, 2, 3, 4, 5 };
            ShadeStream.SetShadeBytes(data, expected);
            byte[] actual = ShadeStream.GetShadeBytes(data);
            CollectionAssert.AreEquivalent(expected, actual.Take(5).ToArray());
        }

        [TestMethod]
        public void GetSetShadeBytesUnicodeTest()
        {
            string expected = "Simon Merlung er Bobbys far";
            Random random = new Random();
            byte[] data = new byte[8*expected.Length];
            random.NextBytes(data);
            ShadeStream.SetShadeBytes(data, UnicodeEncoding.UTF8.GetBytes(expected));
            string actual = UnicodeEncoding.UTF8.GetString(ShadeStream.GetShadeBytes(data));
            Assert.AreEqual(expected, actual);
        }
    }
}
