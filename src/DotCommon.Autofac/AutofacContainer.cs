using Autofac;
using Autofac.Builder;
using DotCommon.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
namespace DotCommon.Autofac
{
    /// <summary>Autofac容器
    /// </summary>
    public class AutofacContainer : IIocContainer
    {
        private ContainerBuilder _builder;
        private IContainer _container;
        public AutofacContainer()
        {
            _builder = new ContainerBuilder();
        }
        public AutofacContainer(ContainerBuilder containerBuilder)
        {
            _builder = containerBuilder;
        }
        public T GetContainer<T>()
        {
            if (_container == null)
            {
                _container = _builder.Build();
            }
            return (T)_container;
        }
        private void SetLifeStyle<T>(IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle> registrationBuilder, DependencyLifeStyle lifeStyle) where T : class
        {
            throw new NotImplementedException();
        }
        public bool IsRegistered(Type type)
        {
            return _container.IsRegistered(type);
        }

        public bool IsRegistered<TType>()
        {
            return _container.IsRegistered<TType>();
        }

        public void Register<T>(DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton) where T : class
        {
            var registrationBuilder = _builder.RegisterType<T>();
            SetLifeStyle(registrationBuilder, lifeStyle);
        }

        public void Register(Type type, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
        {
            var registrationBuilder = _builder.RegisterType(type);
            SetLifeStyle(registrationBuilder, lifeStyle);
        }

        public void Register(Type type, Type impl, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
        {
            var registrationBuilder = _builder.RegisterType(impl).As(type);
            SetLifeStyle(registrationBuilder, lifeStyle);
        }

        public void Register<TType, TImpl>(DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
          where TType : class
          where TImpl : class, TType
        {
            var registrationBuilder = _builder.RegisterType<TType>().As<TImpl>();
            SetLifeStyle(registrationBuilder, lifeStyle);
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
            //需要参数转换
            return _container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        public object Resolve(Type type, object argumentsAsAnonymousType)
        {
            return _container.Resolve(type);
        }


    }
}
