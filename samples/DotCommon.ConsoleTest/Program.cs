using DotCommon.Caching;
using DotCommon.DependencyInjection;
using DotCommon.Encrypt;
using DotCommon.Json4Net;
using DotCommon.Log4Net;
using DotCommon.ProtoBuf;
using DotCommon.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Begin!");
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            IServiceCollection services = new ServiceCollection();
            services
                .AddLogging(l =>
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

            //var logger = provider.GetService<ILogger<Program>>();
            //logger.LogError("Hello!{0}", DateTime.Now.ToString("YYYY-MM-DD HH:mm:ss"));


            //var keyPair = RsaUtil.GenerateFormatKeyPair(keySize: 512);

            //File.WriteAllText(@"D:\public_key", keyPair.PublicKey);
            //File.WriteAllText(@"D:\private_key", keyPair.PrivateKey);


            //var str = "13000C******6";

            ////var id = ShaUtil.GetHex16StringSha256Hash(str);
            //var id = ShaUtil.GetHex16StringSha1Hash(str);
            ////var id = Md5Encryptor.GetMd5(str);

            //Console.WriteLine("Id:{0},长度:{1}", id, id.Length);


            RSA rsa = RSA.Create("RSA");
            //rsa.ImportRSAPrivateKey(Encoding.UTF8.GetBytes(privateKey).AsSpan(), out int r);

            var privateKeyBytes = rsa.ExportRSAPrivateKey();
            var privateKey1 = Convert.ToBase64String(privateKeyBytes);
            var publicKey1 = Convert.ToBase64String(rsa.ExportRSAPublicKey());



            var publicParam = rsa.ExportParameters(false);
            var privateParam = rsa.ExportParameters(true);



            //var p2 = RsaUtil.ReadPublicKeyInfo(publicKey);

            //var p1 = RsaUtil.ReadPublicKeyInfo(publicKey1);

            var privateKey2 = RSAHelper.ExportPrivateKeyPKCS1(privateParam);
            var publicKey2 = RSAHelper.ExportPublicKey(publicParam);

            Console.WriteLine("私钥:{0}", privateKey1);
            Console.WriteLine("私钥2:{0}", privateKey2);
            Console.WriteLine("公钥:{0}", publicKey1);
            Console.WriteLine("公钥2:{0}", publicKey2);







            Console.WriteLine("私钥相等:{0}", privateKey1 == privateKey2);

            Console.WriteLine("公钥相等:{0}", publicKey1 == publicKey2);

            //var key1 = DotCommon.Encrypt.RsaUtil.ReadPrivateKeyInfo(privateKey1);
            //var key2 = DotCommon.Encrypt.RsaUtil.ReadPublicKeyInfo(publicKey1);

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
