using System;

namespace DotCommon.Caching
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct)]
    public class CacheNameAttribute : Attribute
    {
        public string Name { get; }

        public CacheNameAttribute(string name)
        {
            Name = name;
        }
    }
}
