using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotCommon.AspNetCore.Mvc.Conventions
{
    /// <summary>常规控制器选项
    /// </summary>
    public class ConventionalControllerOptions
    {
        private const string DefaultRootPath = "app";

        /// <summary>常规控制器设置集合
        /// </summary>
        public ConventionalControllerSettingList ConventionalControllerSettings { get; }

        /// <summary>FromBody绑定忽略类型
        /// </summary>
        public List<Type> FormBodyBindingIgnoredTypes { get; }

        /// <summary>Ctor
        /// </summary>
        public ConventionalControllerOptions()
        {
            ConventionalControllerSettings = new ConventionalControllerSettingList();

            FormBodyBindingIgnoredTypes = new List<Type>
            {
                typeof(IFormFile)
            };
        }

        /// <summary>创建常规控制器选项
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="optionsAction">控制器设置</param>
        /// <returns></returns>
        public ConventionalControllerOptions Create(Assembly assembly, Action<ConventionalControllerSetting> optionsAction = null)
        {
            var setting = new ConventionalControllerSetting(assembly, DefaultRootPath);
            optionsAction?.Invoke(setting);
            setting.Initialize();
            ConventionalControllerSettings.Add(setting);
            return this;
        }
    }
}
