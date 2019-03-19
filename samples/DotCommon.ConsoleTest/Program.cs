using DotCommon.Encrypt;
using DotCommon.ImageResize;
using DotCommon.Logging;
using DotCommon.Utility;
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
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //IServiceCollection services = new ServiceCollection();
            //services.AddLogging(c =>
            //    {
            //        c.AddLog4Net(new Log4NetProviderOptions());
            //    })
            //    .AddDotCommon()
            //    .AddGenericsMemoryCache()
            //    .AddProtoBuf()
            //    .AddJson4Net();
            //services.AddTransient<LoggerService>();

            //var provider = services.BuildServiceProvider()
            //    .UseDotCommon();
            //var loggerService = provider.GetService<LoggerService>();
            //loggerService.Write();

            //var jsonSerializer = provider.GetService<IJsonSerializer>();

            // var image = Image.FromFile(@"D:\picture\1.jpg");
            // var newImage = ImageResizer.Zoom(image, new ResizeParameter()
            // {
            //     Mode = ResizeMode.Zoom,
            //     Width = 300
            // });
            // var bytes = ImageUtil.ImageToBytes(newImage);

            AesEncryptor aes = new AesEncryptor("MTIzNDU2Nzg5MGFiY2RlZjEyMzQ1Njc4OTBhYmNkZWY=", "MTIzNDU2Nzg5MGFiY2RlZg==");
            aes.KeySize = 256;
            var a = aes.Encrypt("hello");
            Console.WriteLine(a);
            Console.WriteLine("完成");
            // Console.ReadLine();
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
            _logger.LogInformation("生成随机Guid:{0}", DotCommon.Alg.GuidUtil.NewGuidString("N"));
        }
    }

}
