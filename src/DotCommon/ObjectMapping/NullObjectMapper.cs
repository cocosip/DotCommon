using System;

namespace DotCommon.ObjectMapping
{
    public sealed class NullObjectMapper : IObjectMapper
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static NullObjectMapper Instance { get; } = new NullObjectMapper();

        public TDestination Map<TDestination>(object source)
        {
            throw new ArgumentException("ObjectMapping.IObjectMapper should be implemented in order to map objects.");
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            throw new ArgumentException("ObjectMapping.IObjectMapper should be implemented in order to map objects.");
        }
    }
}
