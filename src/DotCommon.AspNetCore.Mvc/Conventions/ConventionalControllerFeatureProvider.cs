using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace DotCommon.AspNetCore.Mvc.Conventions
{
    /// <summary>ConventionalControllerFeatureProvider
    /// </summary>
    public class ConventionalControllerFeatureProvider : ControllerFeatureProvider
    {
        private readonly IDotCommonApplication _dotCommonApplication;
        /// <summary>Ctor
        /// </summary>
        public ConventionalControllerFeatureProvider(IDotCommonApplication dotCommonApplication)
        {
            _dotCommonApplication = dotCommonApplication;
        }

        /// <summary>判断类型是否为控制器
        /// </summary>
        /// <param name="typeInfo">类型信息</param>
        /// <returns></returns>
        protected override bool IsController(TypeInfo typeInfo)
        {
            var configuration = _dotCommonApplication.ServiceProvider
                .GetRequiredService<IOptions<DotCommonAspNetCoreMvcOptions>>().Value
                .ConventionalControllers
                .ConventionalControllerSettings
                .GetSettingOrNull(typeInfo.AsType());
            return configuration != null;
        }
    }
}
