using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cribo.Objects;
using System.Linq;
using System.Text;

namespace CriboTest.Tests
{
    [TestClass]
    public class CriboStreamTest
    {
        [TestMethod]
        public void GetSetCriboBytesTest()
        {
            byte[] data = Enumerable.Range(0, 100000).Select(n=>(byte)0xff).ToArray();
            byte[] expected = new byte[] { 1, 2, 3, 4, 5 };
            CriboStream.SetCriboBytes(data, expected);
            byte[] actual = CriboStream.GetCriboBytes(data);
            CollectionAssert.AreEquivalent(expected, actual.Take(5).ToArray());
        }

        [TestMethod]
        public void GetSetCriboBytesUnicodeTest()
        {
            string expected = "Simon Merlung er Bobbys far";
            Random random = new Random();
            byte[] data = new byte[64000];
            random.NextBytes(data);
            CriboStream.SetCriboBytes(data, UnicodeEncoding.UTF8.GetBytes(expected));
            string actual = UnicodeEncoding.UTF8.GetString(CriboStream.GetCriboBytes(data));
            Assert.AreEqual(expected, actual);
        }
    }
}
