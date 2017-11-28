namespace ShadeTest.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Cribo.Objects;
    using Cribo.Extensions;
    using System.Linq;

    [TestClass]
    public class DictionarySerializableTest
    {
        [TestMethod]
        public void PageTest()
        {
            var page = new DictionarySerializable<string, string>();
            Enumerable.Range(0, 10).ToList().ForEach(i => page.Add("testpage" + i, "testpagecontent" + i));

            var json = page.ToJson();

            var actual = json.ToObjectFromJson<DictionarySerializable<string, string>>();

            Enumerable.Range(0, 10).ToList().ForEach(i => Assert.AreEqual(actual["testpage" + i], "testpagecontent" + i));
        }

        [TestMethod]
        public void PagesTest()
        {
            var pages = new DictionarySerializable<string, DictionarySerializable<string, string>>();

            for (int i = 0; i < 10; i++)
            {
                var page = new DictionarySerializable<string, string>();
                Enumerable.Range(0, 10).ToList().ForEach(n => page.Add("testpage" + n, "testpagecontent" + n));
                pages.Add("page" + i, page);
            }

            var json = pages.ToJson();

            var actual = json.ToObjectFromJson<DictionarySerializable<string, DictionarySerializable<string, string>>>();

            for (int i = 0; i < 10; i++)
            {
                Enumerable.Range(0, 10).ToList().ForEach(n => Assert.AreEqual(actual["page" + i]["testpage" + n], "testpagecontent" + n));
            }
        }

    }
}
