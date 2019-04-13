using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotCommon.AspNetCore.Mvc.Conventions
{
    public class ConventionalControllerOptions
    {
        private const string DefaultRootPath = "app";
        public ConventionalControllerSettingList ConventionalControllerSettings { get; }

        public List<Type> FormBodyBindingIgnoredTypes { get; }

        public ConventionalControllerOptions()
        {
            ConventionalControllerSettings = new ConventionalControllerSettingList();

            FormBodyBindingIgnoredTypes = new List<Type>
            {
                typeof(IFormFile)
            };
        }

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
