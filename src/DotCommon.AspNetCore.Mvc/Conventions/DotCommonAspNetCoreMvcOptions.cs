using Microsoft.Extensions.Options;

namespace DotCommon.AspNetCore.Mvc.Conventions
{
    /// <summary>DotCommon AspNetCoreMvc������
    /// </summary>
    public class DotCommonAspNetCoreMvcOptions
    {
        /// <summary>����������
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
