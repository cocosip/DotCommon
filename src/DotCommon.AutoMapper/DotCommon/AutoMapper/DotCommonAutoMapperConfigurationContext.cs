using System;
using AutoMapper;

namespace DotCommon.AutoMapper
{
    public class DotCommonAutoMapperConfigurationContext : IDotCommonAutoMapperConfigurationContext
    {
        public IMapperConfigurationExpression MapperConfiguration { get; }

        public IServiceProvider ServiceProvider { get; }

        public DotCommonAutoMapperConfigurationContext(
            IMapperConfigurationExpression mapperConfigurationExpression,
            IServiceProvider serviceProvider)
        {
            MapperConfiguration = mapperConfigurationExpression;
            ServiceProvider = serviceProvider;
        }
    }
}
