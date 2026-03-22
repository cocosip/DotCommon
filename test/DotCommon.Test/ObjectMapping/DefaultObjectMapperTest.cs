using System;
using System.Collections.Generic;
using System.Text.Json;
using DotCommon.ObjectMapping;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DotCommon.Test.ObjectMapping
{
    public class DefaultObjectMapperTest
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DefaultObjectMapper _objectMapper;

        public DefaultObjectMapperTest()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IAutoObjectMappingProvider, TestAutoObjectMappingProvider>();
            _serviceProvider = services.BuildServiceProvider();
            _objectMapper = new DefaultObjectMapper(_serviceProvider, _serviceProvider.GetRequiredService<IAutoObjectMappingProvider>());
        }

        [Fact]
        public void Map_WithNullSource_ShouldReturnDefault()
        {
            var result = _objectMapper.Map<SourceClass, DestinationClass>(null!);
            Assert.Null(result);
        }

        [Fact]
        public void Map_ShouldUseAutoMappingProvider()
        {
            var source = new SourceClass { Name = "Test", Age = 25 };
            var result = _objectMapper.Map<SourceClass, DestinationClass>(source);

            Assert.NotNull(result);
            Assert.Equal("Test", result.Name);
            Assert.Equal(25, result.Age);
        }

[Fact]
        public void Map_WithDestination_ShouldReturnMappedResult()
        {
            var source = new SourceClass { Name = "Test", Age = 25 };
            var destination = new DestinationClass { Name = "Original", Age = 0 };
            var result = _objectMapper.Map(source, destination);

            Assert.Equal("Test", result.Name);
            Assert.Equal(25, result.Age);
        }

        [Fact]
        public void Map_WithSpecificMapper_ShouldUseSpecificMapper()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IAutoObjectMappingProvider, TestAutoObjectMappingProvider>();
            services.AddSingleton<IObjectMapper<SourceClass, DestinationClass>, TestSpecificMapper>();
            var serviceProvider = services.BuildServiceProvider();
            var objectMapper = new DefaultObjectMapper(serviceProvider, serviceProvider.GetRequiredService<IAutoObjectMappingProvider>());

            var source = new SourceClass { Name = "Test", Age = 25 };
            var result = objectMapper.Map<SourceClass, DestinationClass>(source);

            Assert.Equal("SpecificMapper: Test", result.Name);
        }

        [Fact]
        public void Map_WithMapToInterface_ShouldUseMapTo()
        {
            var source = new SourceWithMapTo { Value = "TestValue" };
            var result = _objectMapper.Map<SourceWithMapTo, DestinationForMapTo>(source);

            Assert.Equal("MappedTo: TestValue", result.Value);
        }

        [Fact]
        public void Map_WithNullSourceAndDestination_ShouldReturnDefault()
        {
            var result = _objectMapper.Map<SourceClass, DestinationClass>(null!, null!);
            Assert.Null(result);
        }

        [Fact]
        public void Map_WithMapToDestination_ShouldUpdateDestination()
        {
            var source = new SourceWithMapTo { Value = "NewValue" };
            var destination = new DestinationForMapTo { Value = "OldValue" };
            var result = _objectMapper.Map(source, destination);

            Assert.Same(destination, result);
            Assert.Equal("MappedTo: NewValue", result.Value);
        }
    }

    public class ObjectMapperExtensionsTest
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IObjectMapper _objectMapper;

        public ObjectMapperExtensionsTest()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IAutoObjectMappingProvider, TestAutoObjectMappingProvider>();
            _serviceProvider = services.BuildServiceProvider();
            _objectMapper = new DefaultObjectMapper(_serviceProvider, _serviceProvider.GetRequiredService<IAutoObjectMappingProvider>());
        }

        [Fact]
        public void Map_WithTypeParameters_ShouldMapCorrectly()
        {
            var source = new SourceClass { Name = "Test", Age = 30 };
            var result = _objectMapper.Map(typeof(SourceClass), typeof(DestinationClass), source);

            Assert.IsType<DestinationClass>(result);
            var dest = (DestinationClass)result;
            Assert.Equal("Test", dest.Name);
            Assert.Equal(30, dest.Age);
        }

[Fact]
        public void Map_WithDestinationAndTypeParameters_ShouldReturnMappedObject()
        {
            var source = new SourceClass { Name = "Test", Age = 30 };
            var destination = new DestinationClass();
            var result = _objectMapper.Map(typeof(SourceClass), typeof(DestinationClass), source, destination);

            Assert.IsType<DestinationClass>(result);
            var dest = (DestinationClass)result;
            Assert.Equal("Test", dest.Name);
            Assert.Equal(30, dest.Age);
        }
    }

    public class ObjectMapperCollectionTest
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DefaultObjectMapper _objectMapper;

        public ObjectMapperCollectionTest()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IAutoObjectMappingProvider, TestAutoObjectMappingProvider>();
            services.AddSingleton<IObjectMapper<SourceClass, DestinationClass>, TestSpecificMapper>();
            _serviceProvider = services.BuildServiceProvider();
            _objectMapper = new DefaultObjectMapper(_serviceProvider, _serviceProvider.GetRequiredService<IAutoObjectMappingProvider>());
        }

        [Fact]
        public void Map_ListToList_ShouldMapCorrectly()
        {
            var source = new List<SourceClass>
            {
                new SourceClass { Name = "Test1", Age = 20 },
                new SourceClass { Name = "Test2", Age = 30 }
            };

            var result = _objectMapper.Map<List<SourceClass>, List<DestinationClass>>(source);

            Assert.Equal(2, result.Count);
            Assert.Equal("SpecificMapper: Test1", result[0].Name);
            Assert.Equal("SpecificMapper: Test2", result[1].Name);
        }

        [Fact]
        public void Map_ArrayToArray_ShouldMapCorrectly()
        {
            var source = new SourceClass[]
            {
                new SourceClass { Name = "Test1", Age = 20 },
                new SourceClass { Name = "Test2", Age = 30 }
            };

            var result = _objectMapper.Map<SourceClass[], DestinationClass[]>(source);

            Assert.Equal(2, result.Length);
        }

        [Fact]
        public void Map_IEnumerableToList_ShouldMapCorrectly()
        {
            var source = new List<SourceClass>
            {
                new SourceClass { Name = "Test1", Age = 20 }
            };

            var result = _objectMapper.Map<IEnumerable<SourceClass>, List<DestinationClass>>(source);

            Assert.Single(result);
        }
    }

    #region Test Classes

    public class SourceClass
    {
        public string Name { get; set; } = "";
        public int Age { get; set; }
    }

    public class DestinationClass
    {
        public string Name { get; set; } = "";
        public int Age { get; set; }
    }

    public class TestAutoObjectMappingProvider : IAutoObjectMappingProvider
    {
        public TDestination Map<TSource, TDestination>(object source)
        {
            var json = JsonSerializer.Serialize(source);
            return JsonSerializer.Deserialize<TDestination>(json)!;
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            var json = JsonSerializer.Serialize(source);
            return JsonSerializer.Deserialize<TDestination>(json)!;
        }
    }

    public class TestAutoObjectMappingProvider<TContext> : IAutoObjectMappingProvider<TContext>
    {
        public TDestination Map<TSource, TDestination>(object source)
        {
            var json = JsonSerializer.Serialize(source);
            return JsonSerializer.Deserialize<TDestination>(json)!;
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            var json = JsonSerializer.Serialize(source);
            return JsonSerializer.Deserialize<TDestination>(json)!;
        }
    }

    public class TestSpecificMapper : IObjectMapper<SourceClass, DestinationClass>
    {
        public DestinationClass Map(SourceClass source)
        {
            return new DestinationClass
            {
                Name = $"SpecificMapper: {source.Name}",
                Age = source.Age
            };
        }

        public DestinationClass Map(SourceClass source, DestinationClass destination)
        {
            destination.Name = $"SpecificMapper: {source.Name}";
            destination.Age = source.Age;
            return destination;
        }
    }

    public class SourceWithMapTo : IMapTo<DestinationForMapTo>
    {
        public string Value { get; set; } = "";

        public DestinationForMapTo MapTo()
        {
            return new DestinationForMapTo { Value = $"MappedTo: {Value}" };
        }

        public void MapTo(DestinationForMapTo destination)
        {
            destination.Value = $"MappedTo: {Value}";
        }
    }

    public class DestinationForMapTo
    {
        public string Value { get; set; } = "";
    }

    #endregion

    public class DefaultObjectMapperGenericTest
    {
        [Fact]
        public void DefaultObjectMapper_TContext_ShouldMapCorrectly()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IAutoObjectMappingProvider<TestContext>, TestAutoObjectMappingProvider<TestContext>>();
            services.AddSingleton<IAutoObjectMappingProvider>(sp => sp.GetRequiredService<IAutoObjectMappingProvider<TestContext>>());
            var serviceProvider = services.BuildServiceProvider();

            var objectMapper = new DefaultObjectMapper<TestContext>(
                serviceProvider,
                serviceProvider.GetRequiredService<IAutoObjectMappingProvider<TestContext>>());

            var source = new SourceClass { Name = "Test", Age = 25 };
            var result = objectMapper.Map<SourceClass, DestinationClass>(source);

            Assert.NotNull(result);
            Assert.Equal("Test", result.Name);
            Assert.Equal(25, result.Age);
        }

        [Fact]
        public void DefaultObjectMapper_TContext_WithDestination_ShouldMapCorrectly()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IAutoObjectMappingProvider<TestContext>, TestAutoObjectMappingProvider<TestContext>>();
            services.AddSingleton<IAutoObjectMappingProvider>(sp => sp.GetRequiredService<IAutoObjectMappingProvider<TestContext>>());
            var serviceProvider = services.BuildServiceProvider();

            var objectMapper = new DefaultObjectMapper<TestContext>(
                serviceProvider,
                serviceProvider.GetRequiredService<IAutoObjectMappingProvider<TestContext>>());

            var source = new SourceClass { Name = "Test", Age = 25 };
            var destination = new DestinationClass { Name = "Original", Age = 0 };
            var result = objectMapper.Map(source, destination);

            Assert.Equal("Test", result.Name);
            Assert.Equal(25, result.Age);
        }
    }

    public class TestContext { }
}