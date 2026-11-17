using System.Text;
using DotCommon.Crypto.RSA;
using DotCommon.Crypto.SM2;
using DotCommon.Crypto.SM3;
using DotCommon.Crypto.SM4;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for setting up cryptography services in an IServiceCollection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the DotCommon cryptography services to the service collection.
        /// This includes RSA, SM2, SM3, and SM4 encryption services with their default options.
        /// </summary>
        /// <param name="services">The IServiceCollection to add the services to.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
        public static IServiceCollection AddDotCommonCrypto(this IServiceCollection services)
        {
            services
                .Configure<DotCommonSm2EncryptionOptions>(options =>
                {
                    options.DefaultCurve = Sm2EncryptionNames.CurveSm2p256v1;
                })
                .Configure<DotCommonSm4EncryptionOptions>(options =>
                {
                    // SM4 block size is 16 bytes; a shorter default makes any CBC call fail.
                    options.DefaultIv = Encoding.UTF8.GetBytes("DotCommonSm4IV16");
                    options.DefaultMode = Sm4EncryptionNames.ModeECB;
                    options.DefaultPadding = Sm4EncryptionNames.NoPadding;
                })
                .AddTransient<IRSAEncryptionService, RSAEncryptionService>()
                .AddTransient<ISm2EncryptionService, Sm2EncryptionService>()
                .AddTransient<ISm3EncryptionService, Sm3EncryptionService>()
                .AddTransient<ISm4EncryptionService, Sm4EncryptionService>();

            return services;
        }

    }
}