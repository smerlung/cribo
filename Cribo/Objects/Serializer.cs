namespace Cribo.Objects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Cribo.Extensions;

    public static class Serializer
    {
        public static string ToJson(Dictionary<string, Page> store)
        {
            var serializable = new DictionarySerializable<string, DictionarySerializable<string, string>>();
            store
                .ToList()
                .ForEach(n => serializable.Add(n.Value.Key, DictionarySerializable<string, string>.FromDictionary(n.Value.Items)));

            return serializable.ToJson();
        }

        public static Dictionary<string, Page> ToStore(string json)
        {
            var serializable = json.ToObjectFromJson<DictionarySerializable<string, DictionarySerializable<string, string>>>();

            var store = new Dictionary<string, Page>();
            for (int i = 0; i < serializable.Keys.Count; i++ )
            {
                var page = new Page() { Key = serializable.Keys[i], Items = serializable.Values[i].ToDictionary() };
                store.Add(page.Key, page);
            }

            return store;
        }

    }
}
