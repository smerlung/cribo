namespace Cribo.Objects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DictionarySerializable<TKey, TValue> //: IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private bool isdictionaryupdated = true;

        private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
        private List<TKey> keys = new List<TKey>();
        private List<TValue> values = new List<TValue>();

        public List<TKey> Keys { get { return this.keys; } set { this.keys = value; this.UpdateDictionary(); } }

        public List<TValue> Values { get { return this.values; } set { this.values = value; this.UpdateDictionary(); } }

        public void Add(TKey key, TValue value)
        {
            this.dictionary.Add(key, value);
            this.UpdateKeysValues();
        }

        public TValue this[TKey key]
        {
            get
            {
                if (this.isdictionaryupdated)
                {
                    return this.dictionary[key];
                }
                else
                {
                    throw new Exception("Dictionary is not updated");
                }
            }
            set { this.dictionary[key] = value; this.UpdateKeysValues(); }
        }

        private void UpdateKeysValues()
        {
            this.keys = this.dictionary.Keys.ToList();
            this.values = this.dictionary.Values.ToList();
            this.isdictionaryupdated = true;
        }

        private void UpdateDictionary()
        {
            if (this.keys.Count != this.values.Count)
            {
                this.isdictionaryupdated = false;
            }
            else
            {
                this.dictionary = Enumerable.Range(0, this.keys.Count).ToDictionary(i => this.keys[i], i => this.values[i]);
                this.isdictionaryupdated = true;
            }
        }

        public Dictionary<TKey, TValue> ToDictionary()
        {
            var result = new Dictionary<TKey, TValue>();
            for (int i = 0; i < this.Keys.Count; i++)
            {
                result.Add(this.Keys[i], this.Values[i]);
            }

            return result;
        }

        public static DictionarySerializable<TKey, TValue> FromDictionary(Dictionary<TKey, TValue> dictionary)
        {
            var result = new DictionarySerializable<TKey, TValue>();
            dictionary.ToList().ForEach(n => result.Add(n.Key, n.Value));
            return result;
        }
    }
}
