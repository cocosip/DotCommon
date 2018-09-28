using System;

namespace DotCommon.DependencyInjection
{
    public interface IServiceProviderAccessor
    {
        IServiceProvider ServiceProvider { get; }
    }
}
