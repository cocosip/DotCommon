using System;
using DotCommon.Crypto;
using DotCommon.Crypto.SM2;
using DotCommon.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Crypto.Engines;
using Serilog;
using Serilog.Events;

namespace DotCommon.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(
                    path: "logs/serilog-.log",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 14,
                    shared: true,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            try
            {
                Console.WriteLine("Begin!");

                IServiceCollection services = new ServiceCollection();
                services
                    .AddLogging(l =>
                    {
                        l.ClearProviders();
                        l.AddSerilog(Log.Logger, dispose: false);
                    })
                    .AddDotCommon()
                    .AddDotCommonCrypto();

                using var provider = services.BuildServiceProvider();

                var logger = provider.GetRequiredService<ILogger<Program>>();
                logger.LogInformation("Serilog logger initialized.");

                Console.WriteLine(Snowflake.Default.NextId());
                Console.WriteLine(Snowflake.Default.NextId());

                var sm2Service = provider.GetRequiredService<ISm2EncryptionService>();

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
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }

    public class LoggerService
    {
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
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
