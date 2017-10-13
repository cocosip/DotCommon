using DotCommon.Dependency;
using DotCommon.Quartz.Configuration;
using DotCommon.Threading.BackgroundWorkers;
using Quartz;
using System;
using System.Threading.Tasks;

namespace DotCommon.Quartz
{
    public class QuartzScheduleJobManager : BackgroundWorkerBase, IQuartzScheduleJobManager
    {
        private readonly IDotCommonQuartzConfiguration _quartzConfiguration;
        public QuartzScheduleJobManager()
        {
            _quartzConfiguration = IocManager.GetContainer().Resolve<IDotCommonQuartzConfiguration>();
        }

        /// <summary>
        /// </summary>
        public Task ScheduleAsync<TJob>(Action<JobBuilder> configureJob, Action<TriggerBuilder> configureTrigger) where TJob : IJob
        {
            var jobToBuild = JobBuilder.Create<TJob>();
            configureJob(jobToBuild);
            var job = jobToBuild.Build();

            var triggerToBuild = TriggerBuilder.Create();
            configureTrigger(triggerToBuild);
            var trigger = triggerToBuild.Build();

            _quartzConfiguration.Scheduler.ScheduleJob(job, trigger);

            return Task.FromResult(0);
        }


        public override void Start()
        {
            base.Start();
            _quartzConfiguration.Scheduler.Start();
            //if (_backgroundJobConfiguration.IsJobExecutionEnabled)
            //{
            //    _quartzConfiguration.Scheduler.Start();
            //}
            Logger.Info("Started QuartzScheduleJobManager");
        }

        public override void WaitToStop()
        {
            if (_quartzConfiguration.Scheduler != null)
            {
                try
                {
                    _quartzConfiguration.Scheduler.Shutdown(true);
                }
                catch (Exception ex)
                {
                    Logger.Warn(ex.ToString(), ex);
                }
            }

            base.WaitToStop();

            Logger.Info("Stopped QuartzScheduleJobManager");
        }
    }
}
