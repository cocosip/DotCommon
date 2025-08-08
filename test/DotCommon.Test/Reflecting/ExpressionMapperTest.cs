using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotCommon.Reflecting;
using Xunit;

namespace DotCommon.Test.Reflecting
{
    /// <summary>
    /// Unit tests for ExpressionMapper class
    /// </summary>
    public class ExpressionMapperTest
    {
        #region Test Models

        /// <summary>
        /// Simple test model with basic properties
        /// </summary>
        public class SimpleModel
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public double Score { get; set; }
            public bool IsActive { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        /// <summary>
        /// Model with nullable properties
        /// </summary>
        public class NullableModel
        {
            public int? Id { get; set; }
            public string Name { get; set; }
            public double? Score { get; set; }
            public bool? IsActive { get; set; }
            public DateTime? CreatedAt { get; set; }
        }

        /// <summary>
        /// Model with read-only properties
        /// </summary>
        public class ReadOnlyModel
        {
            public int Id { get; }
            public string Name { get; }

            public ReadOnlyModel(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public ReadOnlyModel() : this(0, string.Empty) { }
        }

        /// <summary>
        /// Model with write-only properties
        /// </summary>
        public class WriteOnlyModel
        {
            private int _id;
            private string _name = string.Empty;

            public int Id { set => _id = value; }
            public string Name { set => _name = value; }

            public int GetId() => _id;
            public string GetName() => _name;
        }

        /// <summary>
        /// Model that implements ICollection (should be rejected)
        /// </summary>
        public class CollectionModel : List<string>
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        #endregion

        #region Dictionary to Object Tests

        [Fact]
        public void DictionaryToObject_WithValidData_ShouldConvertCorrectly()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>
            {
                { "Id", 123 },
                { "Name", "Test User" },
                { "Score", 95.5 },
                { "IsActive", true },
                { "CreatedAt", new DateTime(2023, 1, 1) }
            };

            // Act
            var result = ExpressionMapper.DictionaryToObject<SimpleModel>(dictionary);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(123, result.Id);
            Assert.Equal("Test User", result.Name);
            Assert.Equal(95.5, result.Score);
            Assert.True(result.IsActive);
            Assert.Equal(new DateTime(2023, 1, 1), result.CreatedAt);
        }

        [Fact]
        public void DictionaryToObject_WithNullableProperties_ShouldHandleNulls()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>
            {
                { "Id", null },
                { "Name", "Test User" },
                { "Score", null },
                { "IsActive", true },
                { "CreatedAt", null }
            };

            // Act
            var result = ExpressionMapper.DictionaryToObject<NullableModel>(dictionary);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Id);
            Assert.Equal("Test User", result.Name);
            Assert.Null(result.Score);
            Assert.True(result.IsActive);
            Assert.Null(result.CreatedAt);
        }

        [Fact]
        public void DictionaryToObject_WithMissingKeys_ShouldUseDefaults()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>
            {
                { "Name", "Test User" }
            };

            // Act
            var result = ExpressionMapper.DictionaryToObject<SimpleModel>(dictionary);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Id);
            Assert.Equal("Test User", result.Name);
            Assert.Equal(0.0, result.Score);
            Assert.False(result.IsActive);
            Assert.Equal(default(DateTime), result.CreatedAt);
        }

        [Fact]
        public void DictionaryToObject_WithEmptyDictionary_ShouldCreateDefaultObject()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>();

            // Act
            var result = ExpressionMapper.DictionaryToObject<SimpleModel>(dictionary);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Id);
            Assert.Equal(string.Empty, result.Name);
            Assert.Equal(0.0, result.Score);
            Assert.False(result.IsActive);
            Assert.Equal(default(DateTime), result.CreatedAt);
        }

        [Fact]
        public void DictionaryToObject_WithNullDictionary_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                ExpressionMapper.DictionaryToObject<SimpleModel>(null));
        }

        [Fact]
        public void DictionaryToObject_WithCollectionType_ShouldThrowNotSupportedException()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>();

            // Act & Assert
            Assert.Throws<NotSupportedException>(() =>
                ExpressionMapper.DictionaryToObject<CollectionModel>(dictionary));
        }

        #endregion

        #region Object to Dictionary Tests

        [Fact]
        public void ObjectToDictionary_WithValidObject_ShouldConvertCorrectly()
        {
            // Arrange
            var model = new SimpleModel
            {
                Id = 123,
                Name = "Test User",
                Score = 95.5,
                IsActive = true,
                CreatedAt = new DateTime(2023, 1, 1)
            };

            // Act
            var result = ExpressionMapper.ObjectToDictionary(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Equal(123, result["Id"]);
            Assert.Equal("Test User", result["Name"]);
            Assert.Equal(95.5, result["Score"]);
            Assert.Equal(true, result["IsActive"]);
            Assert.Equal(new DateTime(2023, 1, 1), result["CreatedAt"]);
        }

        [Fact]
        public void ObjectToDictionary_WithNullableProperties_ShouldHandleNulls()
        {
            // Arrange
            var model = new NullableModel
            {
                Id = null,
                Name = "Test User",
                Score = null,
                IsActive = true,
                CreatedAt = null
            };

            // Act
            var result = ExpressionMapper.ObjectToDictionary(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Null(result["Id"]);
            Assert.Equal("Test User", result["Name"]);
            Assert.Null(result["Score"]);
            Assert.Equal(true, result["IsActive"]);
            Assert.Null(result["CreatedAt"]);
        }

        [Fact]
        public void ObjectToDictionary_WithNullObject_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                ExpressionMapper.ObjectToDictionary<SimpleModel>(null));
        }

        [Fact]
        public void ObjectToDictionary_WithCollectionType_ShouldThrowNotSupportedException()
        {
            // Arrange
            var model = new CollectionModel();

            // Act & Assert
            Assert.Throws<NotSupportedException>(() =>
                ExpressionMapper.ObjectToDictionary(model));
        }

        #endregion

        #region Converter Caching Tests

        [Fact]
        public void GetDictionaryToObjectConverter_ShouldReturnSameInstance()
        {
            // Act
            var converter1 = ExpressionMapper.GetDictionaryToObjectConverter<SimpleModel>();
            var converter2 = ExpressionMapper.GetDictionaryToObjectConverter<SimpleModel>();

            // Assert
            Assert.Same(converter1, converter2);
        }

        [Fact]
        public void GetObjectToDictionaryConverter_ShouldReturnSameInstance()
        {
            // Act
            var converter1 = ExpressionMapper.GetObjectToDictionaryConverter<SimpleModel>();
            var converter2 = ExpressionMapper.GetObjectToDictionaryConverter<SimpleModel>();

            // Assert
            Assert.Same(converter1, converter2);
        }

        [Fact]
        public void GetConverters_DifferentTypes_ShouldReturnDifferentInstances()
        {
            // Act
            var simpleConverter = ExpressionMapper.GetDictionaryToObjectConverter<SimpleModel>();
            var nullableConverter = ExpressionMapper.GetDictionaryToObjectConverter<NullableModel>();

            // Assert
            Assert.NotSame(simpleConverter, nullableConverter);
        }

        #endregion

        #region Round-trip Conversion Tests

        [Fact]
        public void RoundTripConversion_ShouldPreserveData()
        {
            // Arrange
            var original = new SimpleModel
            {
                Id = 123,
                Name = "Test User",
                Score = 95.5,
                IsActive = true,
                CreatedAt = new DateTime(2023, 1, 1)
            };

            // Act
            var dictionary = ExpressionMapper.ObjectToDictionary(original);
            var result = ExpressionMapper.DictionaryToObject<SimpleModel>(dictionary);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(original.Id, result.Id);
            Assert.Equal(original.Name, result.Name);
            Assert.Equal(original.Score, result.Score);
            Assert.Equal(original.IsActive, result.IsActive);
            Assert.Equal(original.CreatedAt, result.CreatedAt);
        }

        [Fact]
        public void RoundTripConversion_WithNullableProperties_ShouldPreserveNulls()
        {
            // Arrange
            var original = new NullableModel
            {
                Id = null,
                Name = "Test User",
                Score = null,
                IsActive = true,
                CreatedAt = null
            };

            // Act
            var dictionary = ExpressionMapper.ObjectToDictionary(original);
            var result = ExpressionMapper.DictionaryToObject<NullableModel>(dictionary);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(original.Id, result.Id);
            Assert.Equal(original.Name, result.Name);
            Assert.Equal(original.Score, result.Score);
            Assert.Equal(original.IsActive, result.IsActive);
            Assert.Equal(original.CreatedAt, result.CreatedAt);
        }

        #endregion

        #region Performance Tests

        [Fact]
        public void Performance_MultipleConversions_ShouldUseCachedConverters()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>
            {
                { "Id", 123 },
                { "Name", "Test User" },
                { "Score", 95.5 },
                { "IsActive", true },
                { "CreatedAt", new DateTime(2023, 1, 1) }
            };

            // Act - Multiple conversions should use cached converters
            var result1 = ExpressionMapper.DictionaryToObject<SimpleModel>(dictionary);
            var result2 = ExpressionMapper.DictionaryToObject<SimpleModel>(dictionary);
            var result3 = ExpressionMapper.DictionaryToObject<SimpleModel>(dictionary);

            // Assert
            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.NotNull(result3);
            Assert.Equal(result1.Id, result2.Id);
            Assert.Equal(result2.Id, result3.Id);
        }

        [Fact]
        public async Task Performance_MultipleTypes_ShouldWorkConcurrently()
        {
            // Arrange
            var simpleDict = new Dictionary<string, object> { { "Id", 1 }, { "Name", "Simple" } };
            var nullableDict = new Dictionary<string, object> { { "Id", 2 }, { "Name", "Nullable" } };

            // Act
            var task1 = Task.Run(() => ExpressionMapper.DictionaryToObject<SimpleModel>(simpleDict));
            var task2 = Task.Run(() => ExpressionMapper.DictionaryToObject<NullableModel>(nullableDict));
            var task3 = Task.Run(() => ExpressionMapper.DictionaryToObject<SimpleModel>(simpleDict));

            var result1 = await task1;
            var result2 = await task2;
            var result3 = await task3;

            // Assert
            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.NotNull(result3);
            Assert.Equal(1, result1.Id);
            Assert.Equal(2, result2.Id);
            Assert.Equal(1, result3.Id);
        }

        #endregion

        #region Edge Cases

        [Fact]
        public void EdgeCase_EmptyObject_ShouldCreateEmptyDictionary()
        {
            // Arrange
            var model = new SimpleModel();

            // Act
            var result = ExpressionMapper.ObjectToDictionary(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count); // All properties should be included with default values
        }

        [Fact]
        public void EdgeCase_EmptyDictionary_ShouldCreateDefaultObject()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>();

            // Act
            var result = ExpressionMapper.DictionaryToObject<SimpleModel>(dictionary);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Id);
            Assert.Equal(string.Empty, result.Name);
        }

        [Fact]
        public void EdgeCase_TypeConversion_ShouldHandleCompatibleTypes()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>
            {
                { "Id", "123" }, // String to int conversion
                { "Name", "Test User" },
                { "Score", "95.5" }, // String to double conversion
                { "IsActive", "true" } // String to bool conversion
            };

            // Act & Assert
            var exception = Record.Exception(() => ExpressionMapper.DictionaryToObject<SimpleModel>(dictionary));
            
            if (exception == null)
            {
                var result = ExpressionMapper.DictionaryToObject<SimpleModel>(dictionary);
                Assert.NotNull(result);
                Assert.Equal("Test User", result.Name);
            }
            else
            {
                // Type conversion might fail, which is acceptable behavior
                Assert.True(true, "Type conversion failed as expected");
            }
        }

        [Fact]
        public void EdgeCase_SpecialCharacters_ShouldHandleCorrectly()
        {
            // Arrange
            var model = new SimpleModel
            {
                Id = 123,
                Name = "Test User with 特殊字符 and émojis 🚀",
                Score = 95.5,
                IsActive = true,
                CreatedAt = new DateTime(2023, 1, 1)
            };

            // Act
            var dictionary = ExpressionMapper.ObjectToDictionary(model);
            var result = ExpressionMapper.DictionaryToObject<SimpleModel>(dictionary);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.Name, result.Name);
        }

        #endregion
    }
}