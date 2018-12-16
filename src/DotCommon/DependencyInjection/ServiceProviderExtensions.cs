using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace DotCommon.DependencyInjection
{
    public static class ServiceProviderExtensions
    {

        public static object CreateInstance(this IServiceProvider provider, Type type, params object[] args)
        {
            return ActivatorUtilities.CreateInstance(provider, type, args);
        }

        public static T CreateInstance<T>(this IServiceProvider provider, params object[] args)
        {
            return (T)ActivatorUtilities.CreateInstance(provider, typeof(T), args);
        }

        public static object GetServiceByPredicate(this IServiceProvider provider, Type type, Func<object, bool> predicate)
        {
            return provider.GetServices(type).FirstOrDefault(predicate);
        }

        public static T GetServiceByPredicate<T>(this IServiceProvider provider, Func<T, bool> predicate)
        {
            return provider.GetServices<T>().FirstOrDefault(predicate);
        }

        /// <summary>初始化DotCommon
        /// </summary>
        public static IServiceProvider UseDotCommon(this IServiceProvider provider)
        {
            var application = provider.GetRequiredService<IDotCommonApplication>();
            application.Initialize(provider);
            return provider;
        }
    }
}