using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

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

        public static object GetServiceByPredicate(this IServiceProvider provider, Type type, Func<object, bool> predicate)
        {
            return provider.GetServices(type).FirstOrDefault(predicate);
        }

        public static T GetServiceByPredicate<T>(this IServiceProvider provider, Type type, Func<T, bool> predicate)
        {
            return provider.GetServices<T>().FirstOrDefault(predicate);
        }
    }
}
