using System;
using DotCommon;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DotCommon.Test.DependencyInjection
{
    public class ServiceDescriptorExtensionsTest
    {
        [Fact]
        public void NormalizedImplementationInstance_WithNonKeyedService_ShouldReturnInstance()
        {
            var instance = new TestService();
            var descriptor = new ServiceDescriptor(typeof(ITestService), instance);

            var result = descriptor.NormalizedImplementationInstance();

            Assert.Same(instance, result);
        }

        [Fact]
        public void NormalizedImplementationType_WithNonKeyedService_ShouldReturnType()
        {
            var descriptor = new ServiceDescriptor(typeof(ITestService), typeof(TestService), ServiceLifetime.Singleton);

            var result = descriptor.NormalizedImplementationType();

            Assert.Equal(typeof(TestService), result);
        }

        [Fact]
        public void NormalizedImplementationInstance_WithKeyedService_ShouldReturnKeyedInstance()
        {
            var instance = new TestService();
            var descriptor = new ServiceDescriptor(typeof(ITestService), "key", instance);

            var result = descriptor.NormalizedImplementationInstance();

            Assert.Same(instance, result);
        }

        [Fact]
        public void NormalizedImplementationType_WithKeyedService_ShouldReturnKeyedType()
        {
            var descriptor = new ServiceDescriptor(typeof(ITestService), "key", typeof(TestService), ServiceLifetime.Singleton);

            var result = descriptor.NormalizedImplementationType();

            Assert.Equal(typeof(TestService), result);
        }

        private interface ITestService { }
        private class TestService : ITestService { }
    }
}