using AutoMapper;
using System;
using System.Collections.Generic;

namespace DotCommon.AutoMapper
{
    public class AutoMapperConfiguration : IAutoMapperConfiguration
    {
        public List<Action<IMapperConfigurationExpression>> Configurators { get; }

        public AutoMapperConfiguration()
        {
            Configurators = new List<Action<IMapperConfigurationExpression>>();
        }
    }
}
