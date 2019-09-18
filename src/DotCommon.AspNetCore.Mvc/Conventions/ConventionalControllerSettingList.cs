using System;
using System.Collections.Generic;
using System.Linq;

namespace DotCommon.AspNetCore.Mvc.Conventions
{
    /// <summary>��������������б�
    /// </summary>
    public class ConventionalControllerSettingList : List<ConventionalControllerSetting>
    {
        /// <summary>��ȡ����
        /// </summary>
        public ConventionalControllerSetting GetSettingOrNull(Type controllerType)
        {
            return this.FirstOrDefault(controllerSetting => controllerSetting.ControllerTypes.Contains(controllerType));
        }
    }
}
