using AutoMapper;
using System;
using System.Collections.Generic;

namespace DotCommon.AutoMapper
{
    /// <summary>AutoMapper配置
    /// </summary>
    public class AutoMapperConfiguration : IAutoMapperConfiguration
    {
        /// <summary>配置操作
        /// </summary>
        public List<Action<IMapperConfigurationExpression>> Configurators { get; }

        /// <summary>Ctor
        /// </summary>
        public AutoMapperConfiguration()
        {
            Configurators = new List<Action<IMapperConfigurationExpression>>();
        }
    }
}
