using DotCommon.AspNetCore.Mvc.Conventions;
using DotCommon.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace DotCommon.AspNetCore.Mvc
{
    public static class DotCommonMvcOptionsExtensions
    {
        public static void AddDynamicControllers(this MvcOptions options, IServiceCollection services)
        {
            //Use DI to create controllers
            services.AddServiceWhenNull(x => x.ServiceType == typeof(IControllerActivator) && x.ImplementationType == typeof(ServiceBasedControllerActivator), s =>
            {
                s.AddTransient<IControllerActivator, ServiceBasedControllerActivator>();
            });

            //Add feature providers
            var partManager = services.GetSingletonInstance<ApplicationPartManager>();
            var application = services.GetSingletonInstance<IDotCommonApplication>();
            partManager.FeatureProviders.Add(new ConventionalControllerFeatureProvider(application));

            //AddConventions
            AddConventions(options, services);
        }

        private static void AddConventions(MvcOptions options, IServiceCollection services)
        {
            options.Conventions.Add(new ServiceConventionWrapper(services));
        }
    }
}
