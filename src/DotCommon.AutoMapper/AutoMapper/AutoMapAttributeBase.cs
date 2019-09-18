using AutoMapper;
using System;

namespace DotCommon.AutoMapper
{
    /// <summary>自动映射特性
    /// </summary>
    public abstract class AutoMapAttributeBase: Attribute
    {
        /// <summary>目标类型
        /// </summary>
        public Type[] TargetTypes { get; private set; }

        /// <summary>Ctor
        /// </summary>
        protected AutoMapAttributeBase(params Type[] targetTypes)
        {
            TargetTypes = targetTypes;
        }

        /// <summary>创建映射
        /// </summary>
        public abstract void CreateMap(IMapperConfigurationExpression configuration, Type type);
    }
}
