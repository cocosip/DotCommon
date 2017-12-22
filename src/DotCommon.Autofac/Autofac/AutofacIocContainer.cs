using Autofac;
using Autofac.Core;
using DotCommon.Dependency;
using System;
using System.Reflection;

namespace DotCommon.Autofac
{
    public class AutofacIocContainer : IIocContainer
    {
        protected IContainer _container;
        protected ContainerBuilder _builder;

        public AutofacIocContainer()
        {

        }

        public AutofacIocContainer(ContainerBuilder builder)
        {
            _builder = builder;
        }


        public void UseEngine(object engine)
        {
            _container = (IContainer)engine;
        }

        public void Build()
        {
            _container = _builder.Build();
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
            return _container.Resolve<T>(new ResolvedParameter((pi, ctx) => pi.ParameterType == argumentsAsAnonymousType.GetType(), (pi, ctx) => argumentsAsAnonymousType));
        }

        public T ResolveNamed<T>(string serviceName)
        {
            return _container.ResolveNamed<T>(serviceName);
        }


        public object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        public bool IsRegistered(Type type)
        {
            return _container.IsRegistered(type);
        }

        public bool IsRegistered<T>()
        {
            return _container.IsRegistered<T>();
        }

        public void Register<T>(DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton, string serviceName = null, bool isDefault = false) where T : class
        {
            var registrationBuilder = _builder.RegisterType<T>();
            if (serviceName != null)
            {
                registrationBuilder.Named<T>(serviceName);
            }
            if (isDefault)
            {
                registrationBuilder.PreserveExistingDefaults();
            }
            if (lifeStyle != DependencyLifeStyle.Singleton)
            {
                registrationBuilder.InstancePerDependency();
            }
        }

        public void Register(Type type, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton, string serviceName = null, bool propertiesAutowired = true, bool isDefault = false)
        {
            var registrationBuilder = _builder.RegisterType(type);
            if (serviceName != null)
            {
                registrationBuilder.Named(serviceName, type);
            }
            if (propertiesAutowired)
            {
                registrationBuilder.PropertiesAutowired();
            }
            if (isDefault)
            {
                registrationBuilder.PreserveExistingDefaults();
            }
            if (lifeStyle != DependencyLifeStyle.Singleton)
            {
                registrationBuilder.InstancePerDependency();
            }
        }

        public void Register<T>(T impl, bool propertiesAutowired = true, bool isDefault = false) where T : class
        {
            var registrationBuilder = _builder.RegisterInstance(impl);
            if (propertiesAutowired)
            {
                registrationBuilder.PropertiesAutowired();
            }
            if (isDefault)
            {
                registrationBuilder.PreserveExistingDefaults();
            }
        }

        public void Register<TType, TImpl>(DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton, string serviceName = null, bool propertiesAutowired = true, bool isDefault = false)
            where TType : class
            where TImpl : class, TType
        {
            var registrationBuilder = _builder.RegisterType<TImpl>().As<TType>();
            if (serviceName != null)
            {
                registrationBuilder = _builder.RegisterType<TImpl>().Named<TType>(serviceName);
            }

            if (propertiesAutowired)
            {
                registrationBuilder.PropertiesAutowired();
            }
            if (isDefault)
            {
                registrationBuilder.PreserveExistingDefaults();
            }
            if (lifeStyle != DependencyLifeStyle.Singleton)
            {
                registrationBuilder.InstancePerDependency();
            }
        }

        public void Register<TType, TImpl>(TImpl impl, bool propertiesAutowired = true, bool isDefault = false)
         where TType : class
         where TImpl : class, TType
        {
            var registrationBuilder = _builder.RegisterInstance(impl).As<TType>().SingleInstance();
            if (isDefault)
            {
                registrationBuilder.PreserveExistingDefaults();
            }
            if (propertiesAutowired)
            {
                registrationBuilder.PropertiesAutowired();
            }
        }


        public void Register(Type type, Type impl, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton, string serviceName = null, bool propertiesAutowired = false, bool isDefault = false)
        {
            if (type.GetTypeInfo().IsGenericTypeDefinition && type.GetTypeInfo().IsGenericTypeDefinition)
            {
                var genericBuilder = _builder.RegisterGeneric(impl).As(type);
                if (serviceName != null)
                {
                    genericBuilder = _builder.RegisterGeneric(impl).Named(serviceName, type);
                }
                //是否属性注入
                if (propertiesAutowired)
                {
                    genericBuilder.PropertiesAutowired();
                }
                if (lifeStyle != DependencyLifeStyle.Singleton)
                {
                    genericBuilder.InstancePerDependency();
                }
                return;
            }
            var registrationBuilder = _builder.RegisterType(impl).As(type);
            if (serviceName != null)
            {
                registrationBuilder = _builder.RegisterType(impl).Named(serviceName, type);
            }
            if (!isDefault)
            {
                registrationBuilder.PreserveExistingDefaults();
            }
            if (lifeStyle != DependencyLifeStyle.Singleton)
            {
                registrationBuilder.InstancePerDependency();
            }
        }

    }
}
