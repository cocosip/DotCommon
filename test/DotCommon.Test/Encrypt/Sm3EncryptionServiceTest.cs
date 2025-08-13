using DotCommon.Crypto.SM3;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using Xunit;

namespace DotCommon.Test.Encrypt
{
    /// <summary>
    /// Unit tests for SM3 encryption service using dependency injection
    /// </summary>
    public class Sm3EncryptionServiceTest : IDisposable
    {
        private readonly ISm3EncryptionService _sm3EncryptionService;
        private readonly ServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Sm3EncryptionServiceTest"/> class.
        /// Sets up dependency injection container and resolves ISm3EncryptionService.
        /// </summary>
        public Sm3EncryptionServiceTest()
        {
            var services = new ServiceCollection();
            
            // Register SM3 encryption service
            services.AddTransient<ISm3EncryptionService, Sm3EncryptionService>();
            
            _serviceProvider = services.BuildServiceProvider();
            _sm3EncryptionService = _serviceProvider.GetRequiredService<ISm3EncryptionService>();
        }

        /// <summary>
        /// Tests that ISm3EncryptionService can be resolved from dependency injection container
        /// </summary>
        [Fact]
        public void DependencyInjection_ServiceResolution_Test()
        {
            // Assert that the service was resolved successfully
            Assert.NotNull(_sm3EncryptionService);
            Assert.IsAssignableFrom<ISm3EncryptionService>(_sm3EncryptionService);
            Assert.IsType<Sm3EncryptionService>(_sm3EncryptionService);
        }

        /// <summary>
        /// Disposes the service provider to release resources
        /// </summary>
        public void Dispose()
        {
            _serviceProvider?.Dispose();
        }

        [Fact]
        public void GetHash_Bytes_Test()
        {
            var plainText = Encoding.UTF8.GetBytes("abc");
            var hashBytes = _sm3EncryptionService.GetHash(plainText);
            var actualHash = Org.BouncyCastle.Utilities.Encoders.Hex.ToHexString(hashBytes);

            // Assert that the hash is not null or empty and has the correct length (64 characters for 256-bit hash in hex)
            Assert.False(string.IsNullOrEmpty(actualHash));
            Assert.Equal(64, actualHash.Length);
        }

        [Fact]
        public void GetHash_String_Test()
        {
            var plainText = "abc";
            var actualHash = _sm3EncryptionService.GetHash(plainText);

            // Assert that the hash is not null or empty and has the correct length (64 characters for 256-bit hash in hex)
            Assert.False(string.IsNullOrEmpty(actualHash));
            Assert.Equal(64, actualHash.Length);
        }

        [Fact]
        public void GetHash_EmptyString_Test()
        {
            var plainText = "";
            var actualHash = _sm3EncryptionService.GetHash(plainText);

            // Assert that the hash is not null or empty and has the correct length (64 characters for 256-bit hash in hex)
            Assert.False(string.IsNullOrEmpty(actualHash));
            Assert.Equal(64, actualHash.Length);
        }
    }
}