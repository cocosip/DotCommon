﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotCommon.ObjectMapping;

namespace DotCommon.AutoMapper
{
    public class AutoMapperAutoObjectMappingProvider<TContext> : AutoMapperAutoObjectMappingProvider, IAutoObjectMappingProvider<TContext>
    {
        public AutoMapperAutoObjectMappingProvider(IMapperAccessor mapperAccessor)
            : base(mapperAccessor)
        {
        }
    }

    public class AutoMapperAutoObjectMappingProvider : IAutoObjectMappingProvider
    {
        public IMapperAccessor MapperAccessor { get; }

        public AutoMapperAutoObjectMappingProvider(IMapperAccessor mapperAccessor)
        {
            MapperAccessor = mapperAccessor;
        }

        public virtual TDestination Map<TSource, TDestination>(object source)
        {
            return MapperAccessor.Mapper.Map<TDestination>(source);
        }

        public virtual TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return MapperAccessor.Mapper.Map(source, destination);
        }
    }
}
