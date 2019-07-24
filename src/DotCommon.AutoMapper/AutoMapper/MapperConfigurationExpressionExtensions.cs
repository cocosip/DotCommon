using AutoMapper;

namespace DotCommon.AutoMapper
{
    public static class MapperConfigurationExpressionExtensions
    {
        /// <summary>将IAutoMapperConfiguration中的配置应用到某个IMapperConfigurationExpression
        /// </summary>
        public static void ApplyAutoMapperConfiguration(this IMapperConfigurationExpression mapperConfigurationExpression, IAutoMapperConfiguration autoMapperConfiguration)
        {
            foreach (var configurator in autoMapperConfiguration.Configurators)
            {
                configurator(mapperConfigurationExpression);
            }
        }
    }
}
