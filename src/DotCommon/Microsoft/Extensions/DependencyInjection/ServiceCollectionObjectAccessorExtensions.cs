using System;
using System.Linq;
using DotCommon.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> to manage object accessors.
    /// </summary>
    public static class ServiceCollectionObjectAccessorExtensions
    {
        /// <summary>
        /// Tries to add an object accessor for type <typeparamref name="T"/> to the service collection.
        /// If an accessor already exists, it returns the existing one; otherwise, it adds a new one.
        /// </summary>
        /// <typeparam name="T">The type of the object to access.</typeparam>
        /// <param name="services">The service collection to add the accessor to.</param>
        /// <returns>The existing or newly added object accessor.</returns>
        public static ObjectAccessor<T> TryAddObjectAccessor<T>(this IServiceCollection services)
        {
            if (services.Any(s => s.ServiceType == typeof(ObjectAccessor<T>)))
            {
                return services.GetSingletonInstance<ObjectAccessor<T>>();
            }

            return services.AddObjectAccessor<T>();
        }

        /// <summary>
        /// Adds a new object accessor for type <typeparamref name="T"/> to the service collection.
        /// </summary>
        /// <typeparam name="T">The type of the object to access.</typeparam>
        /// <param name="services">The service collection to add the accessor to.</param>
        /// <returns>The newly added object accessor.</returns>
        public static ObjectAccessor<T> AddObjectAccessor<T>(this IServiceCollection services)
        {
            return services.AddObjectAccessor(new ObjectAccessor<T>());
        }

        /// <summary>
        /// Adds a new object accessor for type <typeparamref name="T"/> with a specified object to the service collection.
        /// </summary>
        /// <typeparam name="T">The type of the object to access.</typeparam>
        /// <param name="services">The service collection to add the accessor to.</param>
        /// <param name="obj">The object to be managed by the accessor.</param>
        /// <returns>The newly added object accessor.</returns>
        public static ObjectAccessor<T> AddObjectAccessor<T>(this IServiceCollection services, T obj)
        {
            return services.AddObjectAccessor(new ObjectAccessor<T>(obj));
        }

        /// <summary>
        /// Adds a specified object accessor to the service collection.
        /// Throws an exception if an accessor for the same type already exists.
        /// </summary>
        /// <typeparam name="T">The type of the object to access.</typeparam>
        /// <param name="services">The service collection to add the accessor to.</param>
        /// <param name="accessor">The object accessor to add.</param>
        /// <returns>The added object accessor.</returns>
        /// <exception cref="Exception">Thrown when an accessor for the same type already exists.</exception>
        public static ObjectAccessor<T> AddObjectAccessor<T>(this IServiceCollection services, ObjectAccessor<T> accessor)
        {
            if (services.Any(s => s.ServiceType == typeof(ObjectAccessor<T>)))
            {
                throw new Exception("An object accessor is registered before for type: " + typeof(T).AssemblyQualifiedName);
            }

            // Add to the beginning for fast retrieve
            services.Insert(0, ServiceDescriptor.Singleton(typeof(ObjectAccessor<T>), accessor));
            services.Insert(0, ServiceDescriptor.Singleton(typeof(IObjectAccessor<T>), accessor));

            return accessor;
        }

        /// <summary>
        /// Gets the object of type <typeparamref name="T"/> from the service collection, or null if not found.
        /// </summary>
        /// <typeparam name="T">The type of the object to retrieve.</typeparam>
        /// <param name="services">The service collection to retrieve the object from.</param>
        /// <returns>The object of type <typeparamref name="T"/>, or null if not found.</returns>
        public static T? GetObjectOrNull<T>(this IServiceCollection services)
            where T : class
        {
            return services.GetSingletonInstanceOrNull<IObjectAccessor<T>>()?.Value;
        }

        /// <summary>
        /// Gets the object of type <typeparamref name="T"/> from the service collection.
        /// Throws an exception if the object is not found.
        /// </summary>
        /// <typeparam name="T">The type of the object to retrieve.</typeparam>
        /// <param name="services">The service collection to retrieve the object from.</param>
        /// <returns>The object of type <typeparamref name="T"/>.</returns>
        /// <exception cref="Exception">Thrown when the object of type <typeparamref name="T"/> is not found.</exception>
        public static T GetObject<T>(this IServiceCollection services)
            where T : class
        {
            return services.GetObjectOrNull<T>() ?? throw new Exception($"Could not find an object of {typeof(T).AssemblyQualifiedName} in services. Be sure that you have used AddObjectAccessor before!");
        }
    }
}
