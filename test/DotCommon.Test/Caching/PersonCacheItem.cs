using System;
using System.Collections.Generic;
using System.Text;

namespace DotCommon.Test.Caching
{
    [Serializable]
    public class PersonCacheItem
    {
        public string Name { get; private set; }

        private PersonCacheItem()
        {

        }

        public PersonCacheItem(string name)
        {
            Name = name;
        }
    }
}
