using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace DotCommon.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        public static object GetServiceByArgs(this IServiceProvider provider, Type type, params object[] ctorArgs)
        {
            return ActivatorUtilities.CreateInstance(provider, type, ctorArgs);
        }

        public static T GetServiceByArgs<T>(this IServiceProvider provider, params object[] ctorArgs)
        {
            return (T)GetServiceByArgs(provider, typeof(T), ctorArgs);
        }

        public static object GetServiceByInjectAndArgs(this IServiceProvider provider, Type type, params object[] ctorArgs)
        {
            //如何判断哪些是注入的,哪些是传参的?
            //获取有哪些构造函数
            var newArgs = new object[0];
            var constructorInfos = type.GetConstructors();
            foreach (var constructorInfo in constructorInfos)
            {
                //获取构造函数有哪些参数
                var ctorParameters = constructorInfo.GetParameters();
                if (ctorParameters.Length < ctorArgs.Length)
                {
                    continue;
                }
                //数组长度
                newArgs = new object[ctorParameters.Length];
                //截取后面的几个,前面几个全部用依赖注入注入进去
                var paramInfos = new ParameterInfo[ctorParameters.Length - ctorArgs.Length];
                Array.Copy(ctorParameters, 0, paramInfos, 0, paramInfos.Length);
                var index = 0;
                foreach (var paramInfo in paramInfos)
                {
                    //注入的类型
                    var o = provider.GetService(paramInfo.ParameterType);
                    newArgs.SetValue(o, index);
                    index++;
                }
            }
            //把传入的参数再添加进去
            Array.Copy(ctorArgs, 0, newArgs, newArgs.Length - ctorArgs.Length, ctorArgs.Length);

            return ActivatorUtilities.CreateInstance(provider, type, newArgs);
        }

        public static T GetServiceByInjectAndArgs<T>(this IServiceProvider provider, params object[] ctorArgs)
        {
            return (T)GetServiceByInjectAndArgs(provider, typeof(T), ctorArgs);
        }
    }
}
