using AutoMapper;
using System;
using System.Collections.Generic;

namespace DotCommon.AutoMapper
{
    /// <summary>AutoMapper配置接口
    /// </summary>
    public interface IAutoMapperConfiguration
    {
        /// <summary>配置操作
        /// </summary>
        List<Action<IMapperConfigurationExpression>> Configurators { get; }
    }
}
