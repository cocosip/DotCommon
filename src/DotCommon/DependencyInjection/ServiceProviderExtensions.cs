using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace DotCommon.DependencyInjection
{
    /// <summary>ServiceProvider扩展
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>根据类型使用IServiceProvider创建对象
        /// </summary>
        public static object CreateInstance(this IServiceProvider provider, Type type, params object[] args)
        {
            return ActivatorUtilities.CreateInstance(provider, type, args);
        }

        /// <summary>根据类型使用IServiceProvider创建对象
        /// </summary>
        public static T CreateInstance<T>(this IServiceProvider provider, params object[] args)
        {
            return (T)ActivatorUtilities.CreateInstance(provider, typeof(T), args);
        }

        /// <summary>根据条件获取依赖注入的具体对象
        /// </summary>
        public static object GetServiceByPredicate(this IServiceProvider provider, Type type, Func<object, bool> predicate)
        {
            return provider.GetServices(type).FirstOrDefault(predicate);
        }

        /// <summary>根据条件获取依赖注入的具体对象
        /// </summary>
        public static T GetServiceByPredicate<T>(this IServiceProvider provider, Func<T, bool> predicate)
        {
            return provider.GetServices<T>().FirstOrDefault(predicate);
        }

        /// <summary>初始化DotCommon
        /// </summary>
        public static IServiceProvider ConfigureDotCommon(this IServiceProvider provider)
        {
            var application = provider.GetRequiredService<IDotCommonApplication>();
            application.Initialize(provider);
            return provider;
        }
    }
}