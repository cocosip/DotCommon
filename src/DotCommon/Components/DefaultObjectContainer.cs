#if !NET45
using System;
using Microsoft.Extensions.DependencyInjection;

namespace DotCommon.Components
{
    /// <summary>默认Container
    /// </summary>
    public class DefaultObjectContainer : AbstractObjectContainer
    {
        public override IServiceProvider ApplyServices(IServiceCollection services)
        {
            foreach (var descriptor in CurrentServices)
            {
                services.Add(descriptor);
            }
            Provider = services.BuildServiceProvider();
            return Provider;
        }
    }
}
#endif