using Autofac;
using DotCommon.Configurations;
using DotCommon.Dependency;
using DotCommon.Quartz;
using DotCommon.Threading.BackgroundWorkers;
using Quartz;
using System;
using System.Collections.Generic;
using System.Reflection;
using DotCommon.Extensions;
using DotCommon.Quartz.Configuration;

namespace DotCommon.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            Configurations.Configuration.Create()
                .UseAutofac(builder)
                .RegisterCommonComponent()
                //.RegisterBackgroundWorkers(new List<Assembly>() { typeof(TestBackgroundWorker).Assembly })
                //.RegisterQuartzJobs(new List<Assembly>() { typeof(TestQuartzJob).Assembly })
                .UseLog4Net()
                .UseJson4Net()
                .UseMemoryCache()
                .AutofacBuild();
            Console.WriteLine("初始化完成");
            var container = IocManager.GetContainer();

            //var manager = IocManager.GetContainer().Resolve<IBackgroundWorkerManager>();
            //manager.Add(container.Resolve<TestBackgroundWorker>());
            //manager.Start();

            var manager = IocManager.GetContainer().Resolve<IQuartzScheduleJobManager>();

            var backgroundWorks = IocManager.GetContainer().Resolve(typeof(TestBackgroundWorker)).As<IBackgroundWorker>();


            Console.ReadLine();
        }

        static async void Schedule()
        {
            var manager = IocManager.GetContainer().Resolve<IQuartzScheduleJobManager>();
            await manager.ScheduleAsync<TestQuartzJob>(job =>
             {
                 job.WithIdentity("MyLogJobIdentity", "MyGroup")
                    .WithDescription("A job to simply write logs.");
             }, trigger =>
             {
                 trigger.StartNow()
                    .WithSimpleSchedule(schedule =>
                    {
                        schedule.RepeatForever()
                            .WithIntervalInSeconds(5)
                            .Build();
                    });
             });
        }

    }
}
