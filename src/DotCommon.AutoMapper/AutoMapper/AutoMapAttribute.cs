using AutoMapper;
using DotCommon.Extensions;
using System;

namespace DotCommon.AutoMapper
{
    /// <summary>自动映射特性
    /// </summary>
    public class AutoMapAttribute : AutoMapAttributeBase
    {
        /// <summary>Ctor
        /// </summary>
        public AutoMapAttribute(params Type[] targetTypes)
                  : base(targetTypes)
        {

        }

        /// <summary>创建映射
        /// </summary>
        public override void CreateMap(IMapperConfigurationExpression configuration, Type type)
        {
            if (TargetTypes.IsNullOrEmpty())
            {
                return;
            }
            configuration.CreateAutoAttributeMaps(type, TargetTypes, MemberList.Source);

            foreach (var targetType in TargetTypes)
            {
                configuration.CreateAutoAttributeMaps(targetType, new[] { type }, MemberList.Destination);
                //configuration.CreateMap(type, targetType, MemberList.Source);
                //configuration.CreateMap(targetType, type, MemberList.Destination);
            }
        }
    }
}
