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
using DotCommon.Logging;
using Castle.Windsor;

namespace DotCommon.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            Configurations.Configuration.Create()
                //.UseAutofac(builder)
                .UseAbpContainer(Abp.Dependency.IocManager.Instance.IocContainer)
                .RegisterCommonComponent()
                //.RegisterPeriodicBackgroundWorkers(new List<Assembly>() { typeof(TestBackgroundWorker).Assembly })
                .UseLog4Net()
                .UseJson4Net()
                .UseMemoryCache();
            //.UseQuartz()
            //.RegisterQuartzJobs(new List<Assembly>() { typeof(TestQuartzJob).Assembly })
            //.AutofacBuild()
            //.AddQuartzListener()
            //.BackgroundWorkersAttechAndRun();


            Console.WriteLine("初始化完成");
            var container = IocManager.GetContainer();
            container.Register<ITestClass1, TestClass1Impl1>(DependencyLifeStyle.Transient, "s1");
            container.Register<ITestClass1, TestClass1Impl2>(DependencyLifeStyle.Transient, "s2");
            //Configuration.Instance.AutofacBuild();

            var t1 = container.ResolveNamed<ITestClass1>("s1");
            var t2 = container.ResolveNamed<ITestClass1>("s2");
            Console.WriteLine(t1.GetName());
            Console.WriteLine(t2.GetName());


            // var logger = container.Resolve<ILoggerFactory>().Create("defaultAppender");
            //var workManager = IocManager.GetContainer().Resolve<IBackgroundWorkerManager>();
            //Schedule();
            //workManager.Start();


            //var backgroundWorks = IocManager.GetContainer().Resolve(typeof(TestBackgroundWorker)).As<IBackgroundWorker>();


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
                            .WithIntervalInSeconds(1)
                            .Build();
                    });
             });
        }

    }
}
