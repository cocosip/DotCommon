
using DotCommon.DependencyInjection;
using DotCommon.Json4Net;
using DotCommon.Log4Net;
using DotCommon.Serializing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using DotCommon.Logging;

namespace DotCommon.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Begin!");
            IServiceCollection services = new ServiceCollection();
            services
                .AddLogging(c =>
                {
                    c.AddLog4Net(new Log4NetProviderOptions());
                })
                .AddCommonComponents()
                .AddJson4Net()
                .AddTransient<LoggerService>();

            var provider = services.BuildServiceProvider();
            var loggerService = provider.GetService<LoggerService>();
            loggerService.Write();

            var jsonSerializer = provider.GetService<IJsonSerializer>();



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
            _logger.LogInformation("生成随机Guid:{0}", Guid.NewGuid().ToString());
        }
    }

}
