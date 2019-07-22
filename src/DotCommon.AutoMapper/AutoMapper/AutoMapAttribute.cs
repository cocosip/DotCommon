using AutoMapper;
using DotCommon.Extensions;
using System;

namespace DotCommon.AutoMapper
{
    public class AutoMapAttribute : AutoMapAttributeBase
    {
        public AutoMapAttribute(params Type[] targetTypes)
                  : base(targetTypes)
        {

        }

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
