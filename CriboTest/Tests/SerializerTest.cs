using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cribo.Objects;
using System.Collections.Generic;

namespace ShadeTest.Tests
{
    [TestClass]
    public class SerializerTest
    {
        [TestMethod]
        public void ToJsonTest()
        {
            var items = new Dictionary<string, string>();
            items.Add("username", "rita");
            items.Add("password", "bobby");
            var store = new Dictionary<string, Page>();
            store.Add("test", new Page(){ Key = "test", Items = items});
            store.Add("test2", new Page(){ Key = "test2", Items = items});
            var json = Serializer.ToJson(store);

            var actual = Serializer.ToStore(json);

            Assert.AreEqual(2, actual.Count);
            Assert.IsTrue(actual.ContainsKey("test"));
            Assert.IsTrue(actual.ContainsKey("test2"));

        }
    }
}
