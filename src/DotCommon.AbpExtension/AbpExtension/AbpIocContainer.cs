using Castle.MicroKernel.Registration;
using Castle.Windsor;
using DotCommon.Dependency;
using System;

namespace DotCommon.AbpExtension
{
    public class AbpIocContainer : IIocContainer
    {
        private IWindsorContainer _container { get; set; }
        public AbpIocContainer(IWindsorContainer container)
        {
            _container = container;
        }

        public void UseEngine(object engine)
        {
            _container = (IWindsorContainer)engine;
        }

        public T GetEngine<T>()
        {
            return (T)_container;
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public T Resolve<T>(Type type)
        {
            return (T)_container.Resolve(type);
        }

        public T Resolve<T>(object argumentsAsAnonymousType)
        {
            return _container.Resolve<T>(argumentsAsAnonymousType);
        }

        public object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        public bool IsRegistered(Type type)
        {
            return _container.Kernel.HasComponent(type);
        }

        public bool IsRegistered<T>()
        {
            return _container.Kernel.HasComponent(typeof(T));
        }

        public void Register<T>(DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton, string serviceName = null) where T : class
        {
            var registration = Component.For<T>();
            if (serviceName != null)
            {
                registration.Named(serviceName);
            }
            _container.Register(ApplyLifestyle(registration, lifeStyle));
        }

        public void Register(Type type, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton, string serviceName = null, bool propertiesAutowired = true)
        {
            var registration = Component.For(type);
            if (serviceName != null)
            {
                registration.Named(serviceName);
            }
            _container.Register(ApplyLifestyle(registration, lifeStyle));
        }

        public void Register<T>(T impl, bool propertiesAutowired = true) where T : class
        {
            _container.Register(ApplyLifestyle(Component.For<T>().Instance(impl), DependencyLifeStyle.Singleton));
        }

        public void Register<TType, TImpl>(DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton, string serviceName = null, bool propertiesAutowired = true)
            where TType : class
            where TImpl : class, TType
        {
            var registration = Component.For<TType, TImpl>().ImplementedBy<TImpl>();
            if (serviceName != null)
            {
                registration.Named(serviceName);
            }
            _container.Register(ApplyLifestyle(registration, lifeStyle));
        }

        public void Register<TType, TImpl>(TImpl impl, bool propertiesAutowired = true)
         where TType : class
         where TImpl : class, TType
        {
            _container.Register(ApplyLifestyle(Component.For<TType>().Instance(impl).IsDefault(), DependencyLifeStyle.Singleton));
        }


        public void Register(Type type, Type impl, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton, string serviceName = null, bool propertiesAutowired = true)
        {
            var registration = Component.For(type, impl).ImplementedBy(impl);
            if (serviceName != null)
            {
                registration.Named(serviceName);
            }

            _container.Register(ApplyLifestyle(registration, lifeStyle));
        }

        private static ComponentRegistration<T> ApplyLifestyle<T>(ComponentRegistration<T> registration, DependencyLifeStyle lifeStyle)
           where T : class
        {
            switch (lifeStyle)
            {
                case DependencyLifeStyle.Transient:
                    return registration.LifestyleTransient();
                case DependencyLifeStyle.Singleton:
                    return registration.LifestyleSingleton();
                default:
                    return registration;
            }
        }
    }

}
