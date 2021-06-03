using DotCommon.Caching;
using DotCommon.DependencyInjection;
using DotCommon.ProtoBuf;
using DotCommon.TextJson;
using DotCommon.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;

namespace DotCommon.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Begin!");
            IServiceCollection services = new ServiceCollection();
            services
                .AddLogging(l =>
                {
                    //l.AddLog4Net();
                    l.AddNLog();
                })
                .AddDotCommon()
                .AddGenericsMemoryCache()
                .AddProtoBuf()
                .AddTextJson();

            var provider = services.BuildServiceProvider();


            SnowflakeDistributeId snowflakeDistributeId = new SnowflakeDistributeId(1, 2);

            Console.WriteLine(snowflakeDistributeId.NextId());
            Console.WriteLine(snowflakeDistributeId.NextId());


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
            _logger.LogInformation("生成随机Guid:{0}", GuidUtil.NewGuidString("N"));
        }
    }

}
