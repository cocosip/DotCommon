
using DotCommon.Caching;
using DotCommon.DependencyInjection;
using DotCommon.ImageResize;
using DotCommon.Json4Net;
using DotCommon.Log4Net;
using DotCommon.Logging;
using DotCommon.ProtoBuf;
using DotCommon.Serializing;
using DotCommon.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;

namespace DotCommon.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Begin!");
            IServiceCollection services = new ServiceCollection();
            services.AddLogging(c =>
            {
                c.AddLog4Net(new Log4NetProviderOptions());
            })
            .AddDotCommon()
            .AddGenericsMemoryCache()
            .AddProtoBuf()
            .AddJson4Net();
            services.AddTransient<LoggerService>();

            var provider = services.BuildServiceProvider()
                .UseDotCommon();
            var loggerService = provider.GetService<LoggerService>();
            loggerService.Write();

            var jsonSerializer = provider.GetService<IJsonSerializer>();


            var img1 = Image.FromFile(@"D:\picture\晒家1.jpg");//640x410

            //var img2 = ImageResizer.Zoom(img1, new ResizeParameter()
            //{
            //    Width = 600,
            //    Height=800,
            //    Mode = ResizeMode.Zoom
            //});

            //var img2 = ImageResizer.Rotate90(img1);

            ImageUtil.SaveByQuality(img1, 50, @"D:\picture\Picture1\img_1.jpg");
            //img2.Save(@"D:\picture\Picture1\img_1.jpg");


            img1.Dispose();
            //img2.Dispose();

            Console.WriteLine("完成");

            Console.ReadLine();
        }




    }

    public class LoggerService
    {
        private readonly ILogger _logger;
        public LoggerService(ILogger<LoggerService> logger)
        {
            _logger = logger;
        }

        public void Write()
        {
            _logger.LogWithLevel(LogLevel.Information, "LogWithLevel");
            _logger.LogInformation("生成随机Guid:{0}", DotCommon.Alg.GuidUtil.NewSequentialString("N"));
        }
    }

}
