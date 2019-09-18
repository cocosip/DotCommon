using DotCommon.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotCommon.AspNetCore.Mvc.Conventions
{
    /// <summary>服务转换包装
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

        /// <summary>使用
        /// </summary>
        public void Apply(ApplicationModel application)
        {
            _convention.Value.Apply(application);
        }
    }
}
