using System;
using DotCommon.Crypto;
using DotCommon.Crypto.SM2;
using DotCommon.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Org.BouncyCastle.Crypto.Engines;

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
                .AddDotCommonCrypto();

            var provider = services.BuildServiceProvider();


            Console.WriteLine(Snowflake.Default.NextId());
            Console.WriteLine(Snowflake.Default.NextId());


            var sm2Service = provider.GetService<ISm2EncryptionService>();

            var k = sm2Service.GenerateSm2KeyPair();

            var encrypted = sm2Service.Encrypt(k.ExportPublicKey(), "ABC", mode: SM2Engine.Mode.C1C3C2);
            var decrypted = sm2Service.Decrypt(k.ExportPrivateKey(), encrypted, mode: SM2Engine.Mode.C1C3C2);

            Console.WriteLine("Encrypted:{0}", encrypted);
            Console.WriteLine("Decrypted:{0}", decrypted);

            var signed = sm2Service.Sign(k.ExportPrivateKey(), "123456");
            Console.WriteLine(signed);

            Console.WriteLine(sm2Service.VerifySign(k.ExportPublicKey(), "123456", signed));

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
