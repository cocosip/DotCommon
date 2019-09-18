using DotCommon.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotCommon.AspNetCore.Mvc.Conventions
{
    /// <summary>����ת����װ
    /// </summary>
    //[DisableConventionalRegistration]
    public class ServiceConventionWrapper : IApplicationModelConvention
    {
        private readonly Lazy<IServiceConvention> _convention;

        /// <summary>Ctor
        /// </summary>
        public ServiceConventionWrapper(IServiceCollection services)
        {
            _convention = services.GetRequiredServiceLazy<IServiceConvention>();
        }

        /// <summary>ʹ��
        /// </summary>
        public void Apply(ApplicationModel application)
        {
            _convention.Value.Apply(application);
        }
    }
}
