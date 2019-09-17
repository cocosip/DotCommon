using System;
using System.Linq;
using System.Reflection;

namespace DotCommon.Extensions
{
    /// <summary>类型扩展
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>根据类型获取类型的程序集
        /// </summary>
        public static Assembly GetAssembly(this Type type)
        {
            return type.GetTypeInfo().Assembly;
        }

        /// <summary>根据方法名,参数获取类型方法
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="methodName">方法名</param>
        /// <param name="pParametersCount">参数数量</param>
        /// <param name="pGenericArgumentsCount">泛型参数数量</param>
        /// <returns></returns>
        public static MethodInfo GetMethod(this Type type, string methodName, int pParametersCount = 0, int pGenericArgumentsCount = 0)
        {
            return type
                .GetMethods()
                .Where(m => m.Name == methodName).ToList()
                .Select(m => new
                {
                    Method = m,
                    Params = m.GetParameters(),
                    Args = m.GetGenericArguments()
                })
                .Where(x => x.Params.Length == pParametersCount
                            && x.Args.Length == pGenericArgumentsCount
                ).Select(x => x.Method)
                .First();
        }
    }
}
