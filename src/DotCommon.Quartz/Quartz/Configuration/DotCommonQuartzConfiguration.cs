using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DotCommon.Quartz.Configuration
{
    public class DotCommonQuartzConfiguration : IDotCommonQuartzConfiguration
    {
        private IScheduler _scheduler;

        public IScheduler Scheduler
        {
            get
            {
                if (_scheduler == null)
                {
                    throw new ArgumentException($"Scheduler is null .");
                }
                return _scheduler;
            }
        }

        public async void CreateScheduler()
        {
            _scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        }

    }
}
