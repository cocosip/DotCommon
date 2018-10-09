using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotCommon.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        public static object GetServiceByArgs(this IServiceProvider provider, Type type, params object[] args)
        {
            return ActivatorUtilities.CreateInstance(provider, type, args);
        }

        public static T GetServiceByArgs<T>(this IServiceProvider provider, params object[] ctorArgs)
        {
            return (T)GetServiceByArgs(provider, typeof(T), ctorArgs);
        }

        //private static List<Type> GetMatchCtorArgTypes(Type type, params object[] args)
        //{
        //    var types = new List<Type>();
        //    var constructorInfos = type.GetConstructors();
        //    foreach (var constructorInfo in constructorInfos)
        //    {
        //        //获取构造函数有哪些参数
        //        var ctorParameters = constructorInfo.GetParameters();
        //        //如果构造函数的参数小于传递进来的参数类型,那么肯定是不满足的
        //        if (ctorParameters.Length < args.Length)
        //        {
        //            continue;
        //        }
        //        //1.参数个数大于传递进来的参数,2.参数的最后几个参数的类型与传入参数的类型一致
        //        var skip = ctorParameters.Length - args.Length;
        //        //最后的几个,和传入参数数量一致的类型集合
        //        var ctorParameterTypes = ctorParameters.Select(x => x.ParameterType).Skip(skip).ToArray();

        //        var ctorArgsTypes = args.Select(x => x.GetType()).ToArray();

        //        var isMatch = true;
        //        for (int i = 0; i < ctorParameterTypes.Length; i++)
        //        {
        //            if (ctorParameterTypes[i] != ctorArgsTypes[i])
        //            {
        //                isMatch = false;
        //                break;
        //            }
        //        }
        //        //如果已经匹配,就不匹配下一个构造函数
        //        if (isMatch)
        //        {
        //            //构造函数全部的参数类型
        //            types = ctorParameters.Select(x => x.ParameterType).ToList();
        //            break;
        //        }

        //    }
        //    return types;
        //}

    }
}
