using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using DotCommon.Reflecting;
using Xunit;

namespace DotCommon.Test.Reflecting
{
    /// <summary>
    /// Unit tests for PropertyInfoUtil class.
    /// </summary>
    public class PropertyInfoUtilTest
    {
        #region Test Classes

        /// <summary>
        /// Sample class for testing property operations.
        /// </summary>
        public class SampleClass
        {
            [Required]
            public string Name { get; set; } = string.Empty;

            public int Age { get; set; }

            public string ReadOnlyProperty { get; } = "ReadOnly";

            public string WriteOnlyProperty { set { } }

            public DateTime CreatedDate { get; set; }

            public List<string> Tags { get; set; } = new List<string>();

            private string PrivateProperty { get; set; } = "Private";

            public static string StaticProperty { get; set; } = "Static";
        }

        /// <summary>
        /// Sample nested class for testing.
        /// </summary>
        public class NestedClass
        {
            public SampleClass Sample { get; set; } = new SampleClass();
            public string Description { get; set; } = string.Empty;
        }

        #endregion

        #region Get Properties Tests

        /// <summary>
        /// Tests GetProperties method with Type parameter returns all public instance properties.
        /// </summary>
        [Fact]
        public void GetProperties_WithType_ShouldReturnAllPublicInstanceProperties()
        {
            // Arrange
            var type = typeof(SampleClass);

            // Act
            var properties = PropertyInfoUtil.GetProperties(type);

            // Assert
            Assert.NotNull(properties);
            Assert.True(properties.Count >= 6); // At least 6 public instance properties
            Assert.Contains(properties, p => p.Name == "Name");
            Assert.Contains(properties, p => p.Name == "Age");
            Assert.Contains(properties, p => p.Name == "ReadOnlyProperty");
            Assert.Contains(properties, p => p.Name == "WriteOnlyProperty");
        }

        /// <summary>
        /// Tests GetProperties method with null type throws ArgumentNullException.
        /// </summary>
        [Fact]
        public void GetProperties_WithNullType_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => PropertyInfoUtil.GetProperties((Type)null!));
        }

        /// <summary>
        /// Tests GetProperties method with object parameter returns properties of object's type.
        /// </summary>
        [Fact]
        public void GetProperties_WithObject_ShouldReturnPropertiesOfObjectType()
        {
            // Arrange
            var obj = new SampleClass();

            // Act
            var properties = PropertyInfoUtil.GetProperties(obj);

            // Assert
            Assert.NotNull(properties);
            Assert.True(properties.Count() >= 6);
            Assert.Contains(properties, p => p.Name == "Name");
        }

        /// <summary>
        /// Tests GetProperties method with null object throws ArgumentNullException.
        /// </summary>
        [Fact]
        public void GetProperties_WithNullObject_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => PropertyInfoUtil.GetProperties((object)null!));
        }

        /// <summary>
        /// Tests GetReadableProperties method returns only readable properties.
        /// </summary>
        [Fact]
        public void GetReadableProperties_ShouldReturnOnlyReadableProperties()
        {
            // Arrange
            var type = typeof(SampleClass);

            // Act
            var properties = PropertyInfoUtil.GetReadableProperties(type);

            // Assert
            Assert.NotNull(properties);
            Assert.All(properties, p => Assert.True(p.CanRead));
            Assert.Contains(properties, p => p.Name == "Name");
            Assert.Contains(properties, p => p.Name == "ReadOnlyProperty");
            Assert.DoesNotContain(properties, p => p.Name == "WriteOnlyProperty");
        }

        /// <summary>
        /// Tests GetWritableProperties method returns only writable properties.
        /// </summary>
        [Fact]
        public void GetWritableProperties_ShouldReturnOnlyWritableProperties()
        {
            // Arrange
            var type = typeof(SampleClass);

            // Act
            var properties = PropertyInfoUtil.GetWritableProperties(type);

            // Assert
            Assert.NotNull(properties);
            Assert.All(properties, p => Assert.True(p.CanWrite));
            Assert.Contains(properties, p => p.Name == "Name");
            Assert.Contains(properties, p => p.Name == "WriteOnlyProperty");
            Assert.DoesNotContain(properties, p => p.Name == "ReadOnlyProperty");
        }

        /// <summary>
        /// Tests GetReadWriteProperties method returns only properties with both getter and setter.
        /// </summary>
        [Fact]
        public void GetReadWriteProperties_ShouldReturnOnlyReadWriteProperties()
        {
            // Arrange
            var type = typeof(SampleClass);

            // Act
            var properties = PropertyInfoUtil.GetReadWriteProperties(type);

            // Assert
            Assert.NotNull(properties);
            Assert.All(properties, p => Assert.True(p.CanRead && p.CanWrite));
            Assert.Contains(properties, p => p.Name == "Name");
            Assert.Contains(properties, p => p.Name == "Age");
            Assert.DoesNotContain(properties, p => p.Name == "ReadOnlyProperty");
            Assert.DoesNotContain(properties, p => p.Name == "WriteOnlyProperty");
        }

        #endregion

        #region Find Property Tests

        /// <summary>
        /// Tests FindProperty method with Type parameter finds existing property.
        /// </summary>
        [Fact]
        public void FindProperty_WithTypeAndValidName_ShouldReturnProperty()
        {
            // Arrange
            var type = typeof(SampleClass);
            var propertyName = "Name";

            // Act
            var property = PropertyInfoUtil.FindProperty(type, propertyName);

            // Assert
            Assert.NotNull(property);
            Assert.Equal(propertyName, property.Name);
            Assert.Equal(typeof(string), property.PropertyType);
        }

        /// <summary>
        /// Tests FindProperty method with Type parameter returns null for non-existing property.
        /// </summary>
        [Fact]
        public void FindProperty_WithTypeAndInvalidName_ShouldReturnNull()
        {
            // Arrange
            var type = typeof(SampleClass);
            var propertyName = "NonExistentProperty";

            // Act
            var property = PropertyInfoUtil.FindProperty(type, propertyName);

            // Assert
            Assert.Null(property);
        }

        /// <summary>
        /// Tests FindProperty method with object parameter finds existing property.
        /// </summary>
        [Fact]
        public void FindProperty_WithObjectAndValidName_ShouldReturnProperty()
        {
            // Arrange
            var obj = new SampleClass();
            var propertyName = "Age";

            // Act
            var property = PropertyInfoUtil.FindProperty(obj, propertyName);

            // Assert
            Assert.NotNull(property);
            Assert.Equal(propertyName, property.Name);
            Assert.Equal(typeof(int), property.PropertyType);
        }

        /// <summary>
        /// Tests FindPropertiesByType method finds properties of specified type.
        /// </summary>
        [Fact]
        public void FindPropertiesByType_ShouldReturnPropertiesOfSpecifiedType()
        {
            // Arrange
            var type = typeof(SampleClass);
            var propertyType = typeof(string);

            // Act
            var properties = PropertyInfoUtil.FindPropertiesByType(type, propertyType);

            // Assert
            Assert.NotNull(properties);
            Assert.All(properties, p => Assert.Equal(propertyType, p.PropertyType));
            Assert.Contains(properties, p => p.Name == "Name");
        }

        #endregion

        #region Property Values Tests

        /// <summary>
        /// Tests GetPropertyValue method returns correct property value.
        /// </summary>
        [Fact]
        public void GetPropertyValue_WithValidProperty_ShouldReturnValue()
        {
            // Arrange
            var obj = new SampleClass { Name = "Test", Age = 25 };

            // Act
            var nameValue = PropertyInfoUtil.GetPropertyValue(obj, "Name");
            var ageValue = PropertyInfoUtil.GetPropertyValue(obj, "Age");

            // Assert
            Assert.Equal("Test", nameValue);
            Assert.Equal(25, ageValue);
        }

        /// <summary>
        /// Tests GetPropertyValue generic method returns correctly typed value.
        /// </summary>
        [Fact]
        public void GetPropertyValue_Generic_ShouldReturnTypedValue()
        {
            // Arrange
            var obj = new SampleClass { Name = "Test", Age = 25 };

            // Act
            var nameValue = PropertyInfoUtil.GetPropertyValue<string>(obj, "Name");
            var ageValue = PropertyInfoUtil.GetPropertyValue<int>(obj, "Age");

            // Assert
            Assert.Equal("Test", nameValue);
            Assert.Equal(25, ageValue);
        }

        /// <summary>
        /// Tests GetPropertyValue method throws exception for non-existing property.
        /// </summary>
        [Fact]
        public void GetPropertyValue_WithInvalidProperty_ShouldThrowArgumentException()
        {
            // Arrange
            var obj = new SampleClass();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => PropertyInfoUtil.GetPropertyValue(obj, "NonExistentProperty"));
        }

        /// <summary>
        /// Tests SetPropertyValue method sets property value correctly.
        /// </summary>
        [Fact]
        public void SetPropertyValue_WithValidProperty_ShouldSetValue()
        {
            // Arrange
            var obj = new SampleClass();

            // Act
            PropertyInfoUtil.SetPropertyValue(obj, "Name", "NewName");
            PropertyInfoUtil.SetPropertyValue(obj, "Age", 30);

            // Assert
            Assert.Equal("NewName", obj.Name);
            Assert.Equal(30, obj.Age);
        }

        /// <summary>
        /// Tests SetPropertyValue method throws exception for read-only property.
        /// </summary>
        [Fact]
        public void SetPropertyValue_WithReadOnlyProperty_ShouldThrowArgumentException()
        {
            // Arrange
            var obj = new SampleClass();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => PropertyInfoUtil.SetPropertyValue(obj, "ReadOnlyProperty", "NewValue"));
        }

        #endregion

        #region Property Attributes Tests

        /// <summary>
        /// Tests GetCustomAttributes method returns attributes of specified type.
        /// </summary>
        [Fact]
        public void GetCustomAttributes_ShouldReturnAttributesOfSpecifiedType()
        {
            // Arrange
            var property = typeof(SampleClass).GetProperty("Name")!;

            // Act
            var attributes = PropertyInfoUtil.GetCustomAttributes<RequiredAttribute>(property);

            // Assert
            Assert.NotNull(attributes);
            Assert.Single(attributes);
            Assert.IsType<RequiredAttribute>(attributes[0]);
        }

        /// <summary>
        /// Tests GetCustomAttribute method returns first attribute of specified type.
        /// </summary>
        [Fact]
        public void GetCustomAttribute_ShouldReturnFirstAttributeOfSpecifiedType()
        {
            // Arrange
            var property = typeof(SampleClass).GetProperty("Name")!;

            // Act
            var attribute = PropertyInfoUtil.GetCustomAttribute<RequiredAttribute>(property);

            // Assert
            Assert.NotNull(attribute);
            Assert.IsType<RequiredAttribute>(attribute);
        }

        /// <summary>
        /// Tests HasCustomAttribute method returns true for property with specified attribute.
        /// </summary>
        [Fact]
        public void HasCustomAttribute_WithAttributePresent_ShouldReturnTrue()
        {
            // Arrange
            var property = typeof(SampleClass).GetProperty("Name")!;

            // Act
            var hasAttribute = PropertyInfoUtil.HasCustomAttribute<RequiredAttribute>(property);

            // Assert
            Assert.True(hasAttribute);
        }

        /// <summary>
        /// Tests HasCustomAttribute method returns false for property without specified attribute.
        /// </summary>
        [Fact]
        public void HasCustomAttribute_WithAttributeNotPresent_ShouldReturnFalse()
        {
            // Arrange
            var property = typeof(SampleClass).GetProperty("Age")!;

            // Act
            var hasAttribute = PropertyInfoUtil.HasCustomAttribute<RequiredAttribute>(property);

            // Assert
            Assert.False(hasAttribute);
        }

        #endregion

        #region Property Information Tests

        /// <summary>
        /// Tests IsAutoProperty method returns true for auto-implemented properties.
        /// </summary>
        [Fact]
        public void IsAutoProperty_WithAutoProperty_ShouldReturnTrue()
        {
            // Arrange
            var property = typeof(SampleClass).GetProperty("Name")!;

            // Act
            var isAuto = PropertyInfoUtil.IsAutoProperty(property);

            // Assert
            Assert.True(isAuto);
        }

        /// <summary>
        /// Tests GetPropertyNames method returns array of property names.
        /// </summary>
        [Fact]
        public void GetPropertyNames_WithType_ShouldReturnPropertyNames()
        {
            // Arrange
            var type = typeof(SampleClass);

            // Act
            var propertyNames = PropertyInfoUtil.GetPropertyNames(type);

            // Assert
            Assert.NotNull(propertyNames);
            Assert.Contains("Name", propertyNames);
            Assert.Contains("Age", propertyNames);
            Assert.Contains("CreatedDate", propertyNames);
        }

        /// <summary>
        /// Tests GetPropertyNames method with object parameter returns property names.
        /// </summary>
        [Fact]
        public void GetPropertyNames_WithObject_ShouldReturnPropertyNames()
        {
            // Arrange
            var obj = new SampleClass();

            // Act
            var propertyNames = PropertyInfoUtil.GetPropertyNames(obj);

            // Assert
            Assert.NotNull(propertyNames);
            Assert.Contains("Name", propertyNames);
            Assert.Contains("Age", propertyNames);
        }

        /// <summary>
        /// Tests ToPropertyDictionary method creates dictionary with property names and values.
        /// </summary>
        [Fact]
        public void ToPropertyDictionary_ShouldCreateDictionaryWithPropertyNamesAndValues()
        {
            // Arrange
            var obj = new SampleClass 
            { 
                Name = "Test", 
                Age = 25, 
                CreatedDate = new DateTime(2023, 1, 1) 
            };

            // Act
            var dictionary = PropertyInfoUtil.ToPropertyDictionary(obj);

            // Assert
            Assert.NotNull(dictionary);
            Assert.Equal("Test", dictionary["Name"]);
            Assert.Equal(25, dictionary["Age"]);
            Assert.Equal(new DateTime(2023, 1, 1), dictionary["CreatedDate"]);
            Assert.True(dictionary.ContainsKey("ReadOnlyProperty"));
            Assert.False(dictionary.ContainsKey("WriteOnlyProperty")); // Write-only properties are not readable
        }

        #endregion

        #region Null Parameter Tests

        /// <summary>
        /// Tests methods throw ArgumentNullException for null parameters.
        /// </summary>
        [Fact]
        public void Methods_WithNullParameters_ShouldThrowArgumentNullException()
        {
            // Arrange
            var property = typeof(SampleClass).GetProperty("Name")!;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => PropertyInfoUtil.GetReadableProperties(null!));
            Assert.Throws<ArgumentNullException>(() => PropertyInfoUtil.GetWritableProperties(null!));
            Assert.Throws<ArgumentNullException>(() => PropertyInfoUtil.GetReadWriteProperties(null!));
            Assert.Throws<ArgumentNullException>(() => PropertyInfoUtil.FindProperty((Type)null!, "Name"));
            Assert.Throws<ArgumentNullException>(() => PropertyInfoUtil.FindProperty(typeof(SampleClass), null!));
            Assert.Throws<ArgumentNullException>(() => PropertyInfoUtil.FindProperty((object)null!, "Name"));
            Assert.Throws<ArgumentNullException>(() => PropertyInfoUtil.FindPropertiesByType(null!, typeof(string)));
            Assert.Throws<ArgumentNullException>(() => PropertyInfoUtil.FindPropertiesByType(typeof(SampleClass), null!));
            Assert.Throws<ArgumentNullException>(() => PropertyInfoUtil.GetPropertyValue(null!, "Name"));
            Assert.Throws<ArgumentNullException>(() => PropertyInfoUtil.GetPropertyValue(new SampleClass(), null!));
            Assert.Throws<ArgumentNullException>(() => PropertyInfoUtil.SetPropertyValue(null!, "Name", "value"));
            Assert.Throws<ArgumentNullException>(() => PropertyInfoUtil.SetPropertyValue(new SampleClass(), null!, "value"));
            Assert.Throws<ArgumentNullException>(() => PropertyInfoUtil.GetCustomAttributes<RequiredAttribute>(null!));
            Assert.Throws<ArgumentNullException>(() => PropertyInfoUtil.GetCustomAttribute<RequiredAttribute>(null!));
            Assert.Throws<ArgumentNullException>(() => PropertyInfoUtil.HasCustomAttribute<RequiredAttribute>(null!));
            Assert.Throws<ArgumentNullException>(() => PropertyInfoUtil.IsAutoProperty(null!));
            Assert.Throws<ArgumentNullException>(() => PropertyInfoUtil.GetPropertyNames((Type)null!));
            Assert.Throws<ArgumentNullException>(() => PropertyInfoUtil.GetPropertyNames((object)null!));
            Assert.Throws<ArgumentNullException>(() => PropertyInfoUtil.ToPropertyDictionary(null!));
        }

        #endregion

        #region Edge Cases Tests

        /// <summary>
        /// Tests methods work correctly with empty property names.
        /// </summary>
        [Fact]
        public void Methods_WithEmptyPropertyName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var obj = new SampleClass();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => PropertyInfoUtil.FindProperty(typeof(SampleClass), ""));
            Assert.Throws<ArgumentNullException>(() => PropertyInfoUtil.GetPropertyValue(obj, ""));
            Assert.Throws<ArgumentNullException>(() => PropertyInfoUtil.SetPropertyValue(obj, "", "value"));
        }

        /// <summary>
        /// Tests ToPropertyDictionary handles complex property types correctly.
        /// </summary>
        [Fact]
        public void ToPropertyDictionary_WithComplexPropertyTypes_ShouldHandleCorrectly()
        {
            // Arrange
            var obj = new SampleClass 
            { 
                Tags = new List<string> { "tag1", "tag2" },
                CreatedDate = DateTime.Now
            };

            // Act
            var dictionary = PropertyInfoUtil.ToPropertyDictionary(obj);

            // Assert
            Assert.NotNull(dictionary);
            Assert.IsType<List<string>>(dictionary["Tags"]);
            Assert.IsType<DateTime>(dictionary["CreatedDate"]);
        }

        #endregion
    }
}