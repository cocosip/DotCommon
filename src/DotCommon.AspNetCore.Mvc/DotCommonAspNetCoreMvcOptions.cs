using DotCommon.AspNetCore.Mvc.Conventions;
using Microsoft.Extensions.Options;

namespace DotCommon.AspNetCore.Mvc
{
    public class DotCommonAspNetCoreMvcOptions
    {
        public ConventionalControllerOptions ConventionalControllers { get; }

        public DotCommonAspNetCoreMvcOptions()
        {
            ConventionalControllers = new ConventionalControllerOptions();
        }
    }
}
