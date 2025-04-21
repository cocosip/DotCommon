using System;
using AutoMapper;

namespace DotCommon.AutoMapper
{
    public interface IDotCommonAutoMapperConfigurationContext
    {
        IMapperConfigurationExpression MapperConfiguration { get; }

        IServiceProvider ServiceProvider { get; }
    }
}
