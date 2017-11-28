using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cribo.Objects;
using System.Collections.Generic;

namespace ShadeTest.Tests
{
    [TestClass]
    public class RepositoryTest
    {
        [TestMethod]
        public void CreateTest()
        {
            var store = new Dictionary<string, Page>();
            var repo = new Repository<string, Page>(store);

            Assert.AreEqual(0, store.Count);

            var page = new Page() { Key = "Title", Items = new Dictionary<string, string>() };
            page.Items.Add("Username", "hakon");
            page.Items.Add("Password", "skohorn");
            repo.Create(page);

            Assert.AreEqual(1, store.Count);

        }
    }
}
