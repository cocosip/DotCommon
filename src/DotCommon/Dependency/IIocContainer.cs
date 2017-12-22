using System;

namespace DotCommon.Dependency
{
    /// <summary>容器接口
    /// </summary>
    public interface IIocContainer
    {
        void UseEngine(object container);
        T GetEngine<T>();
        T Resolve<T>();
        T Resolve<T>(Type type);
        T Resolve<T>(object argumentsAsAnonymousType);
        T ResolveNamed<T>(string serviceName);

        object Resolve(Type type);
        bool IsRegistered(Type type);
        bool IsRegistered<T>();

        void Register<T>(DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton, string serviceName = null, bool isDefault = false)
            where T : class;

        void Register(Type type, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton, string serviceName = null, bool propertiesAutowired = true, bool isDefault = false);

        void Register<T>(T impl, bool propertiesAutowired = true, bool isDefault = false) where T : class;

        void Register<TType, TImpl>(DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton, string serviceName = null, bool propertiesAutowired = true, bool isDefault = false)
            where TType : class
            where TImpl : class, TType;

        void Register<TType, TImpl>(TImpl impl, bool propertiesAutowired = true, bool isDefault = false)
            where TType : class
            where TImpl : class, TType;


        void Register(Type type, Type impl, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton, string serviceName = null, bool propertiesAutowired = true, bool isDefault = false);

    }
}
