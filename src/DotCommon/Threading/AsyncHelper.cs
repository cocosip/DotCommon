using System;
using System.Reflection;
using System.Threading.Tasks;

namespace DotCommon.Threading
{
    public static partial class AsyncHelper
    {
        public static bool IsAsync(this MethodInfo method)
        {
            return method.ReturnType.IsTaskOrTaskOfT();
        }

        public static bool IsTaskOrTaskOfT(this Type type)
        {
            return type == typeof(Task) || type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>);
        }

        public static Type UnwrapTask(Type type)
        {
            if (type == typeof(Task))
            {
                return typeof(void);
            }

            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>))
            {
                return type.GenericTypeArguments[0];
            }

            return type;
        }
    }
}
