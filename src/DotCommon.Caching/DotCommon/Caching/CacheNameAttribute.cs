﻿using System;
using System.Linq;
using JetBrains.Annotations;

namespace DotCommon.Caching
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct)]
    public class CacheNameAttribute : Attribute
    {
        public string Name { get; }

        public CacheNameAttribute([NotNull] string name)
        {
            Check.NotNull(name, nameof(name));

            Name = name;
        }

        public static string GetCacheName<TCacheItem>()
        {
            return GetCacheName(typeof(TCacheItem));
        }

        public static string GetCacheName(Type cacheItemType)
        {
            var cacheNameAttribute = cacheItemType
                .GetCustomAttributes(true)
                .OfType<CacheNameAttribute>()
                .FirstOrDefault();

            if (cacheNameAttribute != null)
            {
                return cacheNameAttribute.Name;
            }

            return cacheItemType.FullName!.RemovePostFix("CacheItem")!;
        }
    }
}
