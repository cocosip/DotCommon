using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotCommon.AspNetCore.Mvc.Conventions
{
    /// <summary>���������ѡ��
    /// </summary>
    public class ConventionalControllerOptions
    {
        private const string DefaultRootPath = "app";

        /// <summary>������������ü���
        /// </summary>
        public ConventionalControllerSettingList ConventionalControllerSettings { get; }

        /// <summary>FromBody�󶨺�������
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

        /// <summary>�������������ѡ��
        /// </summary>
        /// <param name="assembly">����</param>
        /// <param name="optionsAction">����������</param>
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
