using AutoMapper;
using DotCommon.Extensions;
using System;

namespace DotCommon.AutoMapper
{
    /// <summary>从某个类型映射特性标签
    /// </summary>
    public class AutoMapFromAttribute : AutoMapAttributeBase
    {
        /// <summary>成员集合
        /// </summary>
        public MemberList MemberList { get; set; } = MemberList.Destination;

        /// <summary>Ctor
        /// </summary>
        public AutoMapFromAttribute(params Type[] targetTypes)
            : base(targetTypes)
        {

        }

        /// <summary>Ctor
        /// </summary>
        public AutoMapFromAttribute(MemberList memberList, params Type[] targetTypes)
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

            foreach (var targetType in TargetTypes)
            {
                //configuration.CreateMap(targetType, type, MemberList);
                configuration.CreateAutoAttributeMaps(targetType, new[] { type }, MemberList);
            }
        }
    }
}
