using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cribo.Objects;

namespace CriboTest.Tests
{
    [TestClass]
    public class SimpleHashTest
    {
        [TestMethod]
        public void CreateTest()
        {
            string password = "qwerty";
            Random random = new Random();
            byte[] salt1 = new byte[32];
            random.NextBytes(salt1);
            byte[] salt2 = new byte[32];
            random.NextBytes(salt2);

            byte[] hash1 = SimpleHash.Create(password, salt1);

            byte[] hash2 = SimpleHash.Create(password, salt2);
            byte[] hash3 = SimpleHash.Create(password, salt2);

            CollectionAssert.AreEquivalent(hash2, hash3);
            CollectionAssert.AreNotEquivalent(hash1, hash2);

        }
    }
}
