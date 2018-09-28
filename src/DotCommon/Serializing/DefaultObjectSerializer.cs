using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotCommon.Serializing
{
    public class DefaultObjectSerializer : IObjectSerializer
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IBinarySerializer _binarySerializer;
        public DefaultObjectSerializer(IServiceProvider serviceProvider, IBinarySerializer binarySerializer)
        {
            _serviceProvider = serviceProvider;
            _binarySerializer = binarySerializer;
        }

        public virtual byte[] Serialize<T>(T obj)
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

        public virtual T Deserialize<T>(byte[] bytes)
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
            return _binarySerializer.Serialize(obj);
        }

        protected virtual T AutoDeserialize<T>(byte[] bytes)
        {
            return _binarySerializer.Deserialize<T>(bytes);
        }
    }
}
