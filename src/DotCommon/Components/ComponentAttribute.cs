using System;

namespace DotCommon.Components
{
    /// <summary>An attribute to indicate a class is a component.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ComponentAttribute : Attribute
    {
        /// <summary>The lifetime of the component.
        /// </summary>
        public LifeStyle LifeStyle { get; private set; }

        /// <summary>Default constructor.
        /// </summary>
        public ComponentAttribute() : this(LifeStyle.Singleton)
        {
        }

        /// <summary>Parameterized constructor.
        /// </summary>
        /// <param name="lifeStyle"></param>
        public ComponentAttribute(LifeStyle lifeStyle)
        {
            LifeStyle = lifeStyle;
        }
    }

    /// <summary>依赖注入不同的生命周期
    /// </summary>
    public enum LifeStyle
    {
        Transient,
        Singleton,
        Scoped
    }
}
