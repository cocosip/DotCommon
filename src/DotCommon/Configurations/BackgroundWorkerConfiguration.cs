using System;
using System.Collections.Generic;

namespace DotCommon.Configurations
{
    public class BackgroundWorkerConfiguration
    {
        /// <summary>后台定时任务的类型
        /// </summary>
        private List<Type> BackgroundWorkTypies = new List<Type>();

        /// <summary>添加类型
        /// </summary>
        public void AddType(Type type)
        {
            if (!BackgroundWorkTypies.Contains(type))
            {
                BackgroundWorkTypies.Add(type);
            }
        }

    }
}
