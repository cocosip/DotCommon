using DotCommon.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotCommon.AspNetCore.Mvc.Conventions
{

    //[DisableConventionalRegistration]
    public class ServiceConventionWrapper : IApplicationModelConvention
    {
        private readonly Lazy<IServiceConvention> _convention;

        public ServiceConventionWrapper(IServiceCollection services)
        {
            _convention = services.GetRequiredServiceLazy<IServiceConvention>();
        }
        public void Apply(ApplicationModel application)
        {
            _convention.Value.Apply(application);
        }
    }
}
