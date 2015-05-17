using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shade.Objects;

namespace ShadeTest.Tests
{
    [TestClass]
    public class CryptoTest
    {
        [TestMethod]
        public void EncryptDecrypt()
        {
            string text = "Simon elsker Stine og Bobby";
            byte[] hash = new byte[]{10,250};
            string cipher = Crypto.Encrypt(text, hash);

            string plaintext = Crypto.Decrypt(cipher, hash);

            Assert.AreEqual(text, plaintext);
        }
    }
}
