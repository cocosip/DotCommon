using AutoMapper;
using System;
using System.Collections.Generic;

namespace DotCommon.AutoMapper
{
    public interface IAutoMapperConfiguration
    {
        List<Action<IMapperConfigurationExpression>> Configurators { get; }

    }
}
