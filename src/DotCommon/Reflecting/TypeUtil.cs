using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DotCommon.Reflecting
{
    /// <summary>类型Util
    /// </summary>
    public static class TypeUtil
    {
        /// <summary>是否为Func类型
        /// </summary>
        public static bool IsFunc(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var type = obj.GetType();
            if (!type.GetTypeInfo().IsGenericType)
            {
                return false;
            }

            return type.GetGenericTypeDefinition() == typeof(Func<>);
        }

        /// <summary>是否为泛型Func类型
        /// </summary>
        public static bool IsFunc<TReturn>(object o)
        {
            return o != null && o.GetType() == typeof(Func<TReturn>);
        }

        /// <summary>是否为原始的扩展
        /// </summary>
        public static bool IsPrimitiveExtended(Type type, bool includeNullables = true, bool includeEnums = false)
        {
            if (IsPrimitiveExtendedInternal(type, includeEnums))
            {
                return true;
            }

            if (includeNullables &&
                type.IsGenericType &&
                type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return IsPrimitiveExtendedInternal(type.GenericTypeArguments[0], includeEnums);
            }

            return false;
        }

        /// <summary>获取第一个可空参数
        /// </summary>
        public static Type GetFirstGenericArgumentIfNullable(Type t)
        {
            if (t == null)
            {
                return default;
            }

            if (t.GetTypeInfo().IsGenericType)
            {
                if (t.GetGenericArguments().Length > 0 && t.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return t.GetGenericArguments().FirstOrDefault();
                }
            }
            return t;
        }

        /// <summary>是否为Async方法
        /// </summary>
        public static bool IsAsync(this MethodInfo method)
        {
            return method.ReturnType.IsTaskOrTaskOfT();
        }

        /// <summary>是否为Task或者泛型Task
        /// </summary>
        public static bool IsTaskOrTaskOfT(this Type type)
        {
            return type == typeof(Task) || (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>));
        }

        private static bool IsPrimitiveExtendedInternal(Type type, bool includeEnums)
        {
            if (type.IsPrimitive)
            {
                return true;
            }

            if (includeEnums && type.IsEnum)
            {
                return true;
            }

            return type == typeof(string) ||
                type == typeof(decimal) ||
                type == typeof(DateTime) ||
                type == typeof(DateTimeOffset) ||
                type == typeof(TimeSpan) ||
                type == typeof(Guid);
        }
    }
}
