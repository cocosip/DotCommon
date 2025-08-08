using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DotCommon.Reflecting
{
    /// <summary>
    /// Type utility class. "类型工具类"
    /// </summary>
    public static class TypeUtil
    {
        /// <summary>
        /// Determines whether the specified object is a generic Func delegate.
        /// </summary>
        /// <param name="obj">The object to check.</param>
        /// <returns>True if the object is a generic Func delegate; otherwise, false.</returns>
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

        /// <summary>
        /// Determines whether the specified object is a Func delegate with the specified return type.
        /// </summary>
        /// <typeparam name="TReturn">The return type of the Func delegate.</typeparam>
        /// <param name="o">The object to check.</param>
        /// <returns>True if the object is a Func delegate with the specified return type; otherwise, false.</returns>
        public static bool IsFunc<TReturn>(object o)
        {
            return o != null && o.GetType() == typeof(Func<TReturn>);
        }

        /// <summary>
        /// Determines whether the specified type is a primitive or extended primitive type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="includeNullables">Whether to include nullable types. Default is true.</param>
        /// <param name="includeEnums">Whether to include enums. Default is false.</param>
        /// <returns>True if the type is primitive or extended; otherwise, false.</returns>
        public static bool IsPrimitiveExtended(Type type, bool includeNullables = true, bool includeEnums = false)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
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

        /// <summary>
        /// Gets the first generic argument if the type is Nullable.
        /// </summary>
        /// <param name="t">The type to check.</param>
        /// <returns>The first generic argument if nullable; otherwise, the original type.</returns>
        public static Type? GetFirstGenericArgumentIfNullable(Type t)
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

        /// <summary>
        /// Determines whether the method is asynchronous (returns Task or Task&lt;T&gt;).
        /// </summary>
        /// <param name="method">The MethodInfo to check.</param>
        /// <returns>True if the method is async; otherwise, false.</returns>
        public static bool IsAsync(this MethodInfo method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            return method.ReturnType.IsTaskOrTaskOfT();
        }

        /// <summary>
        /// Determines whether the type is Task or Task&lt;T&gt;.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if the type is Task or Task&lt;T&gt;; otherwise, false.</returns>
        public static bool IsTaskOrTaskOfT(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return type == typeof(Task) || (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>));
        }

        /// <summary>
        /// Internal helper to determine primitive or extended types.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="includeEnums">Whether to include enums.</param>
        /// <returns>True if the type is primitive or extended; otherwise, false.</returns>
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
