using DotCommon.Caching;
using DotCommon.Crypto;
using DotCommon.DependencyInjection;
using DotCommon.ProtoBuf;
using DotCommon.TextJson;
using DotCommon.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Org.BouncyCastle.Asn1.GM;
using Org.BouncyCastle.Crypto.Agreement;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Text;

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


            var k = SM2Util.GenerateKeyPair();

            var encrypted = SM2Util.Encrypt(k.Item2, "ABC", c132: true);
            var decrypted = SM2Util.Decrypt(k.Item1, encrypted, c132: true);

            Console.WriteLine("Encrypted:{0}", encrypted);
            Console.WriteLine("Decrypted:{0}", decrypted);

            var signed = SM2Util.Sign(k.Item1, "123456");
            Console.WriteLine(signed);

            Console.WriteLine(SM2Util.VerifySign(k.Item2, "123456", signed));


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
