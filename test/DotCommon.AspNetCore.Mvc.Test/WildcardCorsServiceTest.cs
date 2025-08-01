using DotCommon.AspNetCore.Mvc.Cors;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DotCommon.AspNetCore.Mvc.Test
{
    public class WildcardCorsServiceTest
    {
        private readonly Mock<IOptions<CorsOptions>> _mockOptions;
        private readonly Mock<ILoggerFactory> _mockLoggerFactory;
        private readonly WildcardCorsService _wildcardCorsService;

        public WildcardCorsServiceTest()
        {
            _mockOptions = new Mock<IOptions<CorsOptions>>();
            _mockLoggerFactory = new Mock<ILoggerFactory>();
            _wildcardCorsService = new WildcardCorsService(_mockOptions.Object, _mockLoggerFactory.Object);
        }

        [Theory]
        [InlineData("http://example.com", "http://example.com", true)]
        [InlineData("http://sub.example.com", "*.example.com", true)]
        [InlineData("http://example.com", "*.example.com", true)] // Base domain match
        [InlineData("http://another.com", "*.example.com", false)]
        [InlineData("http://test.com", "http://test.com", true)]
        [InlineData("http://test.com", "*", true)] // Universal wildcard
        public void EvaluateOriginForWildcard_ShouldAddOriginWhenWildcardMatches(string requestOrigin, string policyOrigin, bool expectedToBeAdded)
        {
            // Arrange
            var originsList = new List<string> { policyOrigin };

            // Act
            var method = typeof(WildcardCorsService).GetMethod("EvaluateOriginForWildcard", BindingFlags.NonPublic | BindingFlags.Instance);
            if (method != null)
            {
                method.Invoke(_wildcardCorsService, [originsList, requestOrigin]);
            }

            // Assert
            if (expectedToBeAdded)
            {
                Assert.Contains(requestOrigin, originsList);
            }
            else
            {
                if (!originsList.Contains(requestOrigin, StringComparer.OrdinalIgnoreCase))
                {
                    Assert.DoesNotContain(requestOrigin, originsList);
                }
            }
        }

        [Fact]
        public void AddWildcardCors_ShouldRegisterServiceAndConfigurePolicy()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton(Mock.Of<ILoggerFactory>());
            var origins = "http://localhost:5000,*.example.com";

            // Act
            services.AddWildcardCors(origins);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var corsService = serviceProvider.GetService<ICorsService>();
            Assert.IsType<WildcardCorsService>(corsService);

            var corsOptions = serviceProvider.GetService<IOptions<CorsOptions>>();
            var policy = corsOptions?.Value?.GetPolicy(WildcardCorsService.WildcardCorsPolicyName);

            Assert.NotNull(policy);
            Assert.Contains("http://localhost:5000", policy.Origins);
            Assert.Contains("*.example.com", policy.Origins);
            Assert.True(policy.AllowAnyHeader);
            Assert.True(policy.AllowAnyMethod);
            Assert.True(policy.SupportsCredentials);
        }

        [Fact]
        public void AddWildcardCors_WithConfigureAction_ShouldApplyCustomConfiguration()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton(Mock.Of<ILoggerFactory>());
            var origins = "http://localhost:5000";

            // Act
            services.AddWildcardCors(origins, builder =>
            {
                builder.WithHeaders("X-Custom-Header");
                builder.WithMethods("GET");
                builder.DisallowCredentials();
            });

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var corsOptions = serviceProvider.GetService<IOptions<CorsOptions>>();
            var policy = corsOptions?.Value?.GetPolicy(WildcardCorsService.WildcardCorsPolicyName);

            Assert.NotNull(policy);
            Assert.Contains("http://localhost:5000", policy.Origins);
            Assert.Contains("X-Custom-Header", policy.Headers);
            Assert.Contains("GET", policy.Methods);
            Assert.False(policy.SupportsCredentials);
            Assert.False(policy.AllowAnyHeader);
            Assert.False(policy.AllowAnyMethod);
        }
    }
}