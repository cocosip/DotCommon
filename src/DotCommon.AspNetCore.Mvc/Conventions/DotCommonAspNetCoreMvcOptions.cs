using Microsoft.Extensions.Options;

namespace DotCommon.AspNetCore.Mvc.Conventions
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
