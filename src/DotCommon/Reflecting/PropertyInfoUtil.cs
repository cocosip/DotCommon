﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotCommon.Reflecting
{
    /// <summary>
    /// 属性工具类
    /// </summary>
    public static class PropertyInfoUtil
    {
        /// <summary>
        /// 获取某个类型下的所有属性
        /// </summary>
        public static List<PropertyInfo> GetProperties(Type type,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            var properties = type.GetTypeInfo().GetProperties(bindingFlags).ToArray().ToList();
            return properties;
        }

        /// <summary>
        /// 获取某个类型下的所有属性
        /// </summary>
        public static IEnumerable<PropertyInfo> GetProperties(object obj,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            return GetProperties(obj.GetType(), bindingFlags);
        }

    }
}
