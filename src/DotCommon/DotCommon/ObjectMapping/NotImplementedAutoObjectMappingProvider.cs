using System;

namespace DotCommon.ObjectMapping
{
    public sealed class NotImplementedAutoObjectMappingProvider : IAutoObjectMappingProvider
    {
        public TDestination Map<TSource, TDestination>(object source)
        {
            throw new NotImplementedException($"Cannot map from given object ({source}) to {typeof(TDestination).AssemblyQualifiedName}.");
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            throw new NotImplementedException($"Cannot map from {typeof(TSource).AssemblyQualifiedName} to {typeof(TDestination).AssemblyQualifiedName}.");
        }
    }
}
