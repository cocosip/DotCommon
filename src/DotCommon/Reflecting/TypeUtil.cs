using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DotCommon.Reflecting
{
    /// <summary>���͹�����
    /// </summary>
    public static class TypeUtil
    {
        /// <summary>�Ƿ�ΪFunc����ί��
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

        /// <summary>�Ƿ�ΪFunc����ί��
        /// </summary>
        public static bool IsFunc<TReturn>(object obj)
        {
            return obj != null && obj.GetType() == typeof(Func<TReturn>);
        }

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

        public static Type GetFirstGenericArgumentIfNullable(this Type t)
        {
            if (t.GetGenericArguments().Length > 0 && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return t.GetGenericArguments().FirstOrDefault();
            }

            return t;
        }

        /// <summary>����Ƿ�Ϊ�첽����
        /// </summary>
        public static bool IsAsync(this MethodInfo method)
        {
            return method.ReturnType.IsTaskOrTaskOfT();
        }

        /// <summary>�Ƿ�ΪTask���߷���Task
        /// </summary>
        public static bool IsTaskOrTaskOfT(this Type type)
        {
            return type == typeof(Task) || type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>);
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
