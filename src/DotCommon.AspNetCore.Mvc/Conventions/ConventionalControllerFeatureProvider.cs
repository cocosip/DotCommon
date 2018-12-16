using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotCommon.AspNetCore.Mvc.Conventions
{
    public class ConventionalControllerFeatureProvider : ControllerFeatureProvider
    {
        private readonly IDotCommonApplication _dotCommonApplication;
        public ConventionalControllerFeatureProvider(IDotCommonApplication dotCommonApplication)
        {
            _dotCommonApplication = dotCommonApplication;
        }
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
