﻿using System;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace DotCommon.Serialization
{
    public class DefaultObjectSerializer : IObjectSerializer
    {
        private readonly IServiceProvider _serviceProvider;

        public DefaultObjectSerializer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public virtual byte[]? Serialize<T>(T? obj)
        {
            if (obj == null)
            {
                return null;
            }

            //Check if a specific serializer is registered
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

        public virtual T? Deserialize<T>(byte[]? bytes)
        {
            if (bytes == null)
            {
                return default;
            }

            //Check if a specific serializer is registered
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

        protected virtual byte[] AutoSerialize<T>(T obj)
        {
            return JsonSerializer.SerializeToUtf8Bytes(obj);
        }

        protected virtual T? AutoDeserialize<T>(byte[] bytes)
        {
            return JsonSerializer.Deserialize<T>(bytes);
        }
    }
}
