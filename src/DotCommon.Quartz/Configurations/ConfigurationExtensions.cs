using DotCommon.Dependency;
using DotCommon.Quartz;
using DotCommon.Quartz.Configuration;
using Quartz;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotCommon.Configurations
{
    public static class ConfigurationExtensions
    {
        /// <summary>Quartz
        /// </summary>
        public static Configuration UseQuartz(this Configuration configuration)
        {
            var container = IocManager.GetContainer();
            container.Register<IJobListener, DotCommonQuartzJobListener>();

            var quartzConfiguration = new DotCommonQuartzConfiguration();
            quartzConfiguration.CreateScheduler();
            quartzConfiguration.Scheduler.JobFactory = new DotCommonQuartzJobFactory();

            container.Register<IDotCommonQuartzConfiguration, DotCommonQuartzConfiguration>(quartzConfiguration);

            container.Register<IQuartzScheduleJobManager, QuartzScheduleJobManager>();
            return configuration;
        }

        /// <summary>注册Quartz Jobs
        /// </summary>
        public static Configuration RegisterQuartzJobs(this Configuration configuration, List<Assembly> assemblies)
        {
            var container = IocManager.GetContainer();
            var allTypies = assemblies.SelectMany(x => x.GetTypes());
            foreach (var type in allTypies)
            {
                if (typeof(JobBase).IsAssignableFrom(type) && !type.GetTypeInfo().IsAbstract)
                {
                    container.Register(type, DependencyLifeStyle.Transient);
                }
            }
            return configuration;
        }



        /// <summary>添加Quartz监听
        /// </summary>
        public static Configuration AddQuartzListener(this Configuration configuration)
        {
            var container = IocManager.GetContainer();
            container.Resolve<IDotCommonQuartzConfiguration>().Scheduler.ListenerManager.AddJobListener(container.Resolve<IJobListener>());
            return configuration;
        }



    }
}
