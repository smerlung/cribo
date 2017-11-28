namespace Cribo.Objects
{
    using Cribo.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Page : IEntity<string>
    {
        public string Key { get; set; }

        public Dictionary<string, string> Items { get; set; }

        public override string ToString()
        {
            return this.Key;
        }

        public string GetKey()
        {
            return Key;
        }
    }
}
