using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace DotCommon.DependencyInjection
{

    public static class ServiceCollectionCommonExtensions
    {
        /// <summary>
        /// Get singleton instance or null 
        /// </summary>
        public static T GetSingletonInstanceOrNull<T>(this IServiceCollection services)
        {
            return (T)services
                .FirstOrDefault(d => d.ServiceType == typeof(T))?
                .ImplementationInstance;
        }

        /// <summary>
        /// Get singleton instance object
        /// </summary>
        public static T GetSingletonInstance<T>(this IServiceCollection services)
        {
            var service = services.GetSingletonInstanceOrNull<T>();
            if (service == null)
            {
                throw new InvalidOperationException("Could not find singleton service: " + typeof(T).AssemblyQualifiedName);
            }
            return service;
        }

    }
}
