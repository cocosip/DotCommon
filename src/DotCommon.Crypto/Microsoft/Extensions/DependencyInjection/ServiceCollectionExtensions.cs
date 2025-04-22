using System.Text;
using DotCommon.Crypto.SM2;
using DotCommon.Crypto.SM3;
using DotCommon.Crypto.SM4;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDotCommonCrypto(this IServiceCollection services)
        {
            services
                .Configure<DotCommonSm2EncryptionOptions>(options =>
                {
                    options.DefaultCurve = Sm2EncryptionNames.CurveSm2p256v1;
                })
                .Configure<DotCommonSm4EncryptionOptions>(options =>
                {
                    options.DefaultIv = Encoding.UTF8.GetBytes("DotCommon");
                    options.DefaultMode = Sm4EncryptionNames.ModeECB;
                    options.DefaultPadding = Sm4EncryptionNames.NoPadding;
                })
                .AddTransient<ISm2EncryptionService, Sm2EncryptionService>()
                .AddTransient<ISm3EncryptionService, Sm3EncryptionService>()
                .AddTransient<ISm4EncryptionService, Sm4EncryptionService>();

            return services;
        }

    }
}
