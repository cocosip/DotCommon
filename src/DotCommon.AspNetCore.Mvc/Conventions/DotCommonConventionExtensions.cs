using DotCommon.AspNetCore.Mvc.Conventions;
using DotCommon.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace DotCommon.AspNetCore.Mvc
{
    public static class DotCommonConventionExtensions
    {
        /// <summary>动态生成控制器
        /// </summary>
        public static IServiceCollection AddServiceControllers(this IServiceCollection services, Action<DotCommonAspNetCoreMvcOptions> configureOption)
        {
            services.AddTransient<IServiceConvention, ServiceConvention>();

            // AddViewLocalization by default..?
            services.AddServiceWhenNull(x => x.ServiceType == typeof(IActionContextAccessor) && x.ImplementationType == typeof(ActionContextAccessor) && x.Lifetime == ServiceLifetime.Singleton, s =>
            {
                s.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            });

            //Use DI to create controllers
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());
            //Use DI to create view components
            services.Replace(ServiceDescriptor.Singleton<IViewComponentActivator, ServiceBasedViewComponentActivator>());

            //Add feature providers
            var partManager = services.GetSingletonInstance<ApplicationPartManager>();
            var application = services.GetSingletonInstance<IDotCommonApplication>();
            partManager.FeatureProviders.Add(new ConventionalControllerFeatureProvider(application));

            //AddConventions
            services.Configure<MvcOptions>(mvcOpions =>
            {
                AddConventions(mvcOpions, services);
            });

            //相关的控制器添加操作
            services.Configure<DotCommonAspNetCoreMvcOptions>(op =>
            {
                configureOption.Invoke(op);
            });

            return services;
        }

        private static void AddConventions(MvcOptions options, IServiceCollection services)
        {
            options.Conventions.Add(new ServiceConventionWrapper(services));
        }


    }
}