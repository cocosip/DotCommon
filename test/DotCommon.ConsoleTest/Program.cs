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
using DotCommon.Http;
using System.Diagnostics;
using DotCommon.Serializing;
using DotCommon.CastleWindsor;
using Castle.Windsor;

namespace DotCommon.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Begin!");
            // Run();
            //Console.ReadLine();

            var builder = new ContainerBuilder();

            Configurations.Configuration.Create()
                //.UseAutofac(builder)
                .UseCastleWindsor(new WindsorContainer())
                .RegisterCommonComponent()
                //.RegisterPeriodicBackgroundWorkers(new List<Assembly>() { typeof(TestBackgroundWorker).Assembly })
                .UseLog4Net()
                .UseJson4Net()
                .UseMemoryCache()
            //.UseQuartz()
            //.RegisterQuartzJobs(new List<Assembly>() { typeof(TestQuartzJob).Assembly })
            //.AutofacBuild()
            //.AddQuartzListener()
            //.BackgroundWorkersAttechAndRun()
            ;
            //var a = IocManager.GetContainer().Resolve<IJsonSerializer>();
            //Console.WriteLine(a);

            //Console.WriteLine("初始化完成");
            //var container = IocManager.GetContainer();
            //container.Register<ITestClass1, TestClass1Impl1>(DependencyLifeStyle.Transient, "s1");
            //container.Register<ITestClass1, TestClass1Impl2>(DependencyLifeStyle.Transient, "s2");
            ////Configuration.Instance.AutofacBuild();

            //var t1 = container.ResolveNamed<ITestClass1>("s1");
            //var t2 = container.ResolveNamed<ITestClass1>("s2");
            //Console.WriteLine(t1.GetName());
            //Console.WriteLine(t2.GetName());


            // var logger = container.Resolve<ILoggerFactory>().Create("defaultAppender");
            //var workManager = IocManager.GetContainer().Resolve<IBackgroundWorkerManager>();
            //Schedule();
            //workManager.Start();
            Run();

            //var backgroundWorks = IocManager.GetContainer().Resolve(typeof(TestBackgroundWorker)).As<IBackgroundWorker>();


            Console.ReadLine();
        }

        static async void Run()
        {
            IHttpClient client = new DefaultHttpClient();
            Stopwatch watch = new Stopwatch();
            //var builder = RequestBuilder.Instance("http://114.55.101.33:10101/Pda/TokenAuth/Authenticate", RequestConsts.Methods.Post)
            //    .SetPost(PostType.Json, "{\"userNameOrEmailOrPhone\": \"ningbopda00001\",\"password\": \"123456\"}");
            //.SetKeepAlive();
            var builder = RequestBuilder.Instance("https://www.cnblogs.com/daxnet/p", RequestConsts.Methods.Get)
                .SetAuthorization(RequestConsts.AuthenticationSchema.Bearer, @"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjM1IiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6Im5pbmdib3BkYTAwMDAxIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS9hY2Nlc3Njb250cm9sc2VydmljZS8yMDEwLzA3L2NsYWltcy9pZGVudGl0eXByb3ZpZGVyIjoiQVNQLk5FVCBJZGVudGl0eSIsIkFzcE5ldC5JZGVudGl0eS5TZWN1cml0eVN0YW1wIjoiMWNlYTMxNDktYTdjYi00YWJkLWFiMDYtMzgxOGZiMWFlYWIxIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjpbIkFkbWluIiwiMGEyNGJlYjM1OTM1NDRhMDgwMGUxNDk1ODZlOGE5N2EiXSwic3ViIjoiMzUiLCJqdGkiOiI4MWZhMGYwYS01MTFmLTQxNWItOWE0Zi1hMWRjYjY0YTQwZjYiLCJpYXQiOjE1MTA4MDM2ODcsIm5iZiI6MTUxMDgwMzY4NywiZXhwIjoxNTEwODkwMDg3LCJpc3MiOiJBYnBaZXJvVGVtcGxhdGUiLCJhdWQiOiJBYnBaZXJvVGVtcGxhdGUifQ.qrGgGbtLwhThqC61QDbkZkx7Uv-keAiHbtvCOylV_T4");
            var response = await client.ExecuteAsync(builder);
            Console.WriteLine(response.GetResponseString());
            //for (int i = 0; i < 1000; i++)
            //{
            //    watch.Start();
            //    var builder = RequestBuilder.Instance("http://www.baidu.com", RequestConsts.Methods.Get)
            //        .SetKeepAlive();
            //    var response = await client.ExecuteAsync(builder);

            //    watch.Stop();
            //    //Console.WriteLine(response.GetResponseString());
            //    Console.WriteLine($"第{i}次,花费:{watch.Elapsed}");
            //    watch.Reset();
            //}
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
