using System;
using DotCommon.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DotCommon.Test.DependencyInjection
{
    public class ServiceCollectionObjectAccessorExtensionsTest
    {
        [Fact]
        public void TryAddObjectAccessor_FirstTime_ShouldAdd()
        {
            var services = new ServiceCollection();
            var accessor = services.TryAddObjectAccessor<string>();

            Assert.NotNull(accessor);
            Assert.Null(accessor.Value);
        }

        [Fact]
        public void TryAddObjectAccessor_SecondTime_ShouldReturnExisting()
        {
            var services = new ServiceCollection();
            var accessor1 = services.TryAddObjectAccessor<string>();
            accessor1.Value = "first";

            var accessor2 = services.TryAddObjectAccessor<string>();

            Assert.Same(accessor1, accessor2);
            Assert.Equal("first", accessor2.Value);
        }

        [Fact]
        public void AddObjectAccessor_ShouldAdd()
        {
            var services = new ServiceCollection();
            var accessor = services.AddObjectAccessor<string>();

            Assert.NotNull(accessor);
            Assert.Null(accessor.Value);
        }

        [Fact]
        public void AddObjectAccessor_WithValue_ShouldSetValue()
        {
            var services = new ServiceCollection();
            var accessor = services.AddObjectAccessor("test");

            Assert.NotNull(accessor);
            Assert.Equal("test", accessor.Value);
        }

        [Fact]
        public void AddObjectAccessor_WithAccessor_ShouldAdd()
        {
            var services = new ServiceCollection();
            var accessor = new ObjectAccessor<int> { Value = 42 };
            var result = services.AddObjectAccessor(accessor);

            Assert.Same(accessor, result);
        }

        [Fact]
        public void AddObjectAccessor_Duplicate_ShouldThrow()
        {
            var services = new ServiceCollection();
            services.AddObjectAccessor<string>();

            Assert.Throws<Exception>(() => services.AddObjectAccessor<string>());
        }

        [Fact]
        public void GetObjectOrNull_WithAccessor_ShouldReturnValue()
        {
            var services = new ServiceCollection();
            services.AddObjectAccessor("test");

            var result = services.GetObjectOrNull<string>();

            Assert.Equal("test", result);
        }

        [Fact]
        public void GetObjectOrNull_WithoutAccessor_ShouldReturnNull()
        {
            var services = new ServiceCollection();
            var result = services.GetObjectOrNull<string>();
            Assert.Null(result);
        }

        [Fact]
        public void GetObject_WithAccessor_ShouldReturnValue()
        {
            var services = new ServiceCollection();
            services.AddObjectAccessor("test");

            var result = services.GetObject<string>();

            Assert.Equal("test", result);
        }

        [Fact]
        public void GetObject_WithoutAccessor_ShouldThrow()
        {
            var services = new ServiceCollection();
            Assert.Throws<Exception>(() => services.GetObject<string>());
        }
    }

    public class ServiceCollectionCommonExtensionsTests
    {
        [Fact]
        public void IsAdded_WithRegisteredService_ShouldReturnTrue()
        {
            var services = new ServiceCollection();
            services.AddSingleton<string>();

            Assert.True(services.IsAdded<string>());
        }

        [Fact]
        public void IsAdded_WithoutRegisteredService_ShouldReturnFalse()
        {
            var services = new ServiceCollection();
            Assert.False(services.IsAdded<string>());
        }

        [Fact]
        public void GetSingletonInstanceOrNull_WithSingleton_ShouldReturn()
        {
            var services = new ServiceCollection();
            services.AddSingleton(new TestService { Value = 42 });

            var result = services.GetSingletonInstanceOrNull<TestService>();

            Assert.NotNull(result);
            Assert.Equal(42, result.Value);
        }

        [Fact]
        public void GetSingletonInstanceOrNull_WithoutSingleton_ShouldReturnNull()
        {
            var services = new ServiceCollection();
            var result = services.GetSingletonInstanceOrNull<TestService>();
            Assert.Null(result);
        }

        [Fact]
        public void GetSingletonInstance_WithSingleton_ShouldReturn()
        {
            var services = new ServiceCollection();
            services.AddSingleton(new TestService { Value = 42 });

            var result = services.GetSingletonInstance<TestService>();

            Assert.NotNull(result);
            Assert.Equal(42, result.Value);
        }

        [Fact]
        public void GetSingletonInstance_WithoutSingleton_ShouldThrow()
        {
            var services = new ServiceCollection();
            Assert.Throws<InvalidOperationException>(() => services.GetSingletonInstance<TestService>());
        }

        private class TestService
        {
            public int Value { get; set; }
        }
    }
}