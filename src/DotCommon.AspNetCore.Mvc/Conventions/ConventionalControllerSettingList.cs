using System;
using System.Collections.Generic;
using System.Linq;

namespace DotCommon.AspNetCore.Mvc.Conventions
{
    /// <summary>常规控制器设置列表
    /// </summary>
    public class ConventionalControllerSettingList : List<ConventionalControllerSetting>
    {
        /// <summary>获取设置
        /// </summary>
        public ConventionalControllerSetting GetSettingOrNull(Type controllerType)
        {
            return this.FirstOrDefault(controllerSetting => controllerSetting.ControllerTypes.Contains(controllerType));
        }
    }
}
