﻿using System;

namespace DotCommon.Caching
{
    /// <summary>
    /// CacheName attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct)]
    public class CacheNameAttribute : Attribute
    {
        /// <summary>缓存名
        /// </summary>
        public string Name { get; }

        /// <summary>Ctor
        /// </summary>
        public CacheNameAttribute(string name)
        {
            Name = name;
        }
    }
}
