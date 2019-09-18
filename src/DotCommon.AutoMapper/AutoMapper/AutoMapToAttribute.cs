using AutoMapper;
using DotCommon.Extensions;
using System;

namespace DotCommon.AutoMapper
{
    /// <summary>映射到目标特性标签
    /// </summary>
    public class AutoMapToAttribute : AutoMapAttributeBase
    {
        /// <summary>成员集合
        /// </summary>
        public MemberList MemberList { get; set; } = MemberList.Source;

        /// <summary>Ctor
        /// </summary>
        public AutoMapToAttribute(params Type[] targetTypes)
            : base(targetTypes)
        {

        }

        /// <summary>Ctor
        /// </summary>
        public AutoMapToAttribute(MemberList memberList, params Type[] targetTypes)
            : this(targetTypes)
        {
            MemberList = memberList;
        }

        /// <summary>创建映射
        /// </summary>
        public override void CreateMap(IMapperConfigurationExpression configuration, Type type)
        {
            if (TargetTypes.IsNullOrEmpty())
            {
                return;
            }

            configuration.CreateAutoAttributeMaps(type, TargetTypes, MemberList);
            //foreach (var targetType in TargetTypes)
            //{
            //    configuration.CreateMap(type, targetType, MemberList);
            //}
        }
    }
}
