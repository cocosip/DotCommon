using System;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace DotCommon.Serialization
{
    /// <summary>
    /// Default implementation of object serializer
    /// </summary>
    public class DefaultObjectSerializer : IObjectSerializer
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving specific type serializers</param>
        public DefaultObjectSerializer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Serializes an object to byte array
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="obj">Object instance to serialize</param>
        /// <returns>Serialized byte array, or null if object is null</returns>
        public virtual byte[]? Serialize<T>(T? obj)
        {
            if (obj == null)
            {
                return null;
            }

            // Check if a specific serializer is registered
            using (var scope = _serviceProvider.CreateScope())
            {
                var specificSerializer = scope.ServiceProvider.GetService<IObjectSerializer<T>>();
                if (specificSerializer != null)
                {
                    return specificSerializer.Serialize(obj);
                }
            }

            return AutoSerialize(obj);
        }

        /// <summary>
        /// Deserializes byte array to object
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize</typeparam>
        /// <param name="bytes">Byte array to deserialize</param>
        /// <returns>Deserialized object instance, or default value if byte array is null</returns>
        public virtual T? Deserialize<T>(byte[]? bytes)
        {
            if (bytes == null)
            {
                return default;
            }

            // Check if a specific serializer is registered
            using (var scope = _serviceProvider.CreateScope())
            {
                var specificSerializer = scope.ServiceProvider.GetService<IObjectSerializer<T>>();
                if (specificSerializer != null)
                {
                    return specificSerializer.Deserialize(bytes);
                }
            }

            return AutoDeserialize<T>(bytes);
        }

        /// <summary>
        /// Automatically serialize object to UTF-8 bytes
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="obj">Object instance to serialize</param>
        /// <returns>Serialized byte array</returns>
        protected virtual byte[] AutoSerialize<T>(T obj)
        {
            return JsonSerializer.SerializeToUtf8Bytes(obj);
        }

        /// <summary>
        /// Automatically deserialize byte array to object
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize</typeparam>
        /// <param name="bytes">Byte array to deserialize</param>
        /// <returns>Deserialized object instance</returns>
        protected virtual T? AutoDeserialize<T>(byte[] bytes)
        {
            return JsonSerializer.Deserialize<T>(bytes);
        }
    }
}
