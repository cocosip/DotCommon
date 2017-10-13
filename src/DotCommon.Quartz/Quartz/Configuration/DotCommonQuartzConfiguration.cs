﻿using Quartz;
using Quartz.Impl;

namespace DotCommon.Quartz.Configuration
{
    public class DotCommonQuartzConfiguration
    {
        public DotCommonQuartzConfiguration()
        {

        }
        public IScheduler Scheduler { get; private set; }

        public async void InitScheduler() => Scheduler = await StdSchedulerFactory.GetDefaultScheduler();

    }
}
