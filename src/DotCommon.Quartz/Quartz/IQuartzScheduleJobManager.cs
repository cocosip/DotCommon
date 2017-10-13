using Quartz;
using System;
using System.Threading.Tasks;

namespace DotCommon.Quartz
{
    public interface IQuartzScheduleJobManager
    {
        /// <summary>调度任务
        /// </summary>
        Task ScheduleAsync<TJob>(Action<JobBuilder> configureJob, Action<TriggerBuilder> configureTrigger) where TJob : IJob;
    }
}
