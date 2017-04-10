using Autofac;
using Autofac.Builder;
using DotCommon.Dependency;

namespace DotCommon.Autofac
{
    public static class AutofacBuilderExtensions
    {
        public static void SetLifeStyle<T>(this IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle> registrationBuilder, DependencyLifeStyle lifeStyle) where T : class
        {
            if (lifeStyle == DependencyLifeStyle.Singleton)
            {
                registrationBuilder.SingleInstance();
            }
            if (lifeStyle == DependencyLifeStyle.Scoped)
            {
                registrationBuilder.InstancePerLifetimeScope();
            }
        }

       
        public static void Test(this ContainerBuilder builder)
        {

        }

    }
}
