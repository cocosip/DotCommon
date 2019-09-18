using Microsoft.Extensions.Options;

namespace DotCommon.AspNetCore.Mvc.Conventions
{
    /// <summary>DotCommon AspNetCoreMvcµƒ≈‰÷√
    /// </summary>
    public class DotCommonAspNetCoreMvcOptions
    {
        /// <summary>øÿ÷∆∆˜≈‰÷√
        /// </summary>
        public ConventionalControllerOptions ConventionalControllers { get; }

        /// <summary>Ctor
        /// </summary>
        public DotCommonAspNetCoreMvcOptions()
        {
            ConventionalControllers = new ConventionalControllerOptions();
        }
    }
}
