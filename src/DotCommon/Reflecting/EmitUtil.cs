﻿using System;
using System.Reflection;

namespace DotCommon.Reflecting
{
    /// <summary>
    /// Emit工具类
    /// </summary>
    public static class EmitUtil
    {
        /// <summary>
        /// 可空类型判断
        /// </summary>
        public static bool IsNullable(Type type)
        {
            return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// 获取可空类型的真实类型
        /// </summary>
        public static Type GetNullableArg0(Type type)
        {
            return IsNullable(type) ? type.GenericTypeArguments[0] : type;
        }
    }
}
