using DotCommon.Caching;
using DotCommon.DependencyInjection;
using DotCommon.Encrypt;
using DotCommon.Json4Net;
using DotCommon.Log4Net;
using DotCommon.Logging;
using DotCommon.ProtoBuf;
using DotCommon.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.IO;

namespace DotCommon.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Begin!");
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            IServiceCollection services = new ServiceCollection();
            services.AddLogging(l =>
            {
                l.AddLog4Net();
                l.AddNLog();
            })
                .AddDotCommon()
                .AddGenericsMemoryCache()
                .AddProtoBuf()
                .AddJson4Net();
            //services.AddTransient<LoggerService>();
            var provider = services.BuildServiceProvider()
                .ConfigureDotCommon();
            //.ConfigureLog4Net();
            NLog.LogManager.LoadConfiguration("nlog.config");

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


            //AesEncryptor aes = new AesEncryptor("MTIzNDU2Nzg5MGFiY2RlZjEyMzQ1Njc4OTBhYmNkZWY=", "MTIzNDU2Nzg5MGFiY2RlZg==");
            //aes.KeySize = 256;
            //var a = aes.Encrypt("hello");
            //Console.WriteLine(a);

            var logger = provider.GetService<ILogger<Program>>();
            logger.LogError("Hello!{0}", DateTime.Now.ToString("YYYY-MM-DD HH:mm:ss"));


            var (publicKey, privateKey) = RsaKeyUtil.GenerateFormatKeyPair(keySize: 512);

            File.WriteAllText(@"D:\public_key", publicKey);
            File.WriteAllText(@"D:\private_key", privateKey);

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
            _logger.LogInformation("生成随机Guid:{0}", GuidUtil.NewGuidString("N"));
        }
    }

}
