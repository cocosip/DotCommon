using Nito.AsyncEx;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace DotCommon.Threading
{

    /// <summary>异步工具类
    /// </summary>
    public static class AsyncHelper
    {
        /// <summary>判断方法是否为异步方法
        /// </summary>
        /// <param name="method">方法</param>
        /// <returns></returns>
        public static bool IsAsync(this MethodInfo method)
        {
            return method.ReturnType.IsTaskOrTaskOfT();
        }

        /// <summary>判断是否为Task或者泛型Task
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static bool IsTaskOrTaskOfT(this Type type)
        {
            return type == typeof(Task) || type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>);
        }

        /// <summary>不包裹类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
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

        /// <summary>同步调用异步方法
        /// </summary>
        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return AsyncContext.Run(func);
        }


        /// <summary>同步调用异步方法,无返回值
        /// </summary>
        public static void RunSync(Func<Task> action)
        {
            AsyncContext.Run(action);
        }
    }
}
