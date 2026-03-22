using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotCommon.Reflecting;
using Xunit;

namespace DotCommon.Test.Reflecting
{
    /// <summary>
    /// Unit tests for the ReflectionUtil class.
    /// </summary>
    public class ReflectionUtilTest
    {
        #region Test Classes and Attributes

        /// <summary>
        /// Sample attribute for testing purposes.
        /// </summary>
        [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
        public class SampleAttribute : Attribute
        {
            public string Value { get; set; }

            public SampleAttribute(string value)
            {
                Value = value;
            }
        }

        /// <summary>
        /// Sample class for testing reflection operations.
        /// </summary>
        [Sample("ClassAttribute")]
        public class SampleClass
        {
            [Sample("PropertyAttribute")]
            public string Name { get; set; }

            public int Age { get; set; }

            public SampleNestedClass Nested { get; set; }

            [Sample("MethodAttribute")]
            public void SampleMethod()
            {
            }

            public string get_TestProperty()
            {
                return "test";
            }
        }

        /// <summary>
        /// Nested class for testing property path operations.
        /// </summary>
        public class SampleNestedClass
        {
            public string NestedProperty { get; set; }
        }

        /// <summary>
        /// Generic interface for testing generic type assignability.
        /// </summary>
        /// <typeparam name="T">The generic type parameter</typeparam>
        public interface IGenericInterface<T>
        {
        }

        /// <summary>
        /// Class implementing generic interface for testing.
        /// </summary>
        public class GenericImplementation : IGenericInterface<string>
        {
        }

        /// <summary>
        /// Generic class for testing generic type assignability.
        /// </summary>
        /// <typeparam name="T">The generic type parameter</typeparam>
        public class GenericClass<T>
        {
        }

        #endregion

        #region IsAssignableToGenericType Tests

        /// <summary>
        /// Tests that IsAssignableToGenericType returns true when a class implements a generic interface.
        /// </summary>
        [Fact]
        public void IsAssignableToGenericType_ClassImplementsGenericInterface_ReturnsTrue()
        {
            // Arrange
            var givenType = typeof(GenericImplementation);
            var genericType = typeof(IGenericInterface<>);

            // Act
            var result = ReflectionUtil.IsAssignableToGenericType(givenType, genericType);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Tests that IsAssignableToGenericType returns true when a generic class is checked against its generic definition.
        /// </summary>
        [Fact]
        public void IsAssignableToGenericType_GenericClassToGenericDefinition_ReturnsTrue()
        {
            // Arrange
            var givenType = typeof(GenericClass<string>);
            var genericType = typeof(GenericClass<>);

            // Act
            var result = ReflectionUtil.IsAssignableToGenericType(givenType, genericType);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Tests that IsAssignableToGenericType returns false when types are not related.
        /// </summary>
        [Fact]
        public void IsAssignableToGenericType_UnrelatedTypes_ReturnsFalse()
        {
            // Arrange
            var givenType = typeof(string);
            var genericType = typeof(IGenericInterface<>);

            // Act
            var result = ReflectionUtil.IsAssignableToGenericType(givenType, genericType);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Tests that IsAssignableToGenericType throws ArgumentNullException when givenType is null.
        /// </summary>
        [Fact]
        public void IsAssignableToGenericType_GivenTypeIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            Type givenType = null;
            var genericType = typeof(IGenericInterface<>);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ReflectionUtil.IsAssignableToGenericType(givenType, genericType));
        }

        /// <summary>
        /// Tests that IsAssignableToGenericType throws ArgumentNullException when genericType is null.
        /// </summary>
        [Fact]
        public void IsAssignableToGenericType_GenericTypeIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var givenType = typeof(string);
            Type genericType = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ReflectionUtil.IsAssignableToGenericType(givenType, genericType));
        }

        #endregion

        #region GetAttributesOfMemberAndDeclaringType Tests

        /// <summary>
        /// Tests that GetAttributesOfMemberAndDeclaringType returns attributes from both member and declaring type.
        /// </summary>
        [Fact]
        public void GetAttributesOfMemberAndDeclaringType_MemberWithAttributes_ReturnsAllAttributes()
        {
            // Arrange
            var memberInfo = typeof(SampleClass).GetProperty("Name");

            // Act
            var attributes = ReflectionUtil.GetAttributesOfMemberAndDeclaringType(memberInfo);

            // Assert
            Assert.NotEmpty(attributes);
            Assert.Contains(attributes, attr => attr is SampleAttribute sampleAttr && sampleAttr.Value == "PropertyAttribute");
            Assert.Contains(attributes, attr => attr is SampleAttribute sampleAttr && sampleAttr.Value == "ClassAttribute");
        }

        /// <summary>
        /// Tests that GetAttributesOfMemberAndDeclaringType throws ArgumentNullException when memberInfo is null.
        /// </summary>
        [Fact]
        public void GetAttributesOfMemberAndDeclaringType_MemberInfoIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            MemberInfo memberInfo = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ReflectionUtil.GetAttributesOfMemberAndDeclaringType(memberInfo));
        }

        #endregion

        #region GetAttributesOfMemberAndType Tests

        /// <summary>
        /// Tests that GetAttributesOfMemberAndType returns attributes from both member and specified type.
        /// </summary>
        [Fact]
        public void GetAttributesOfMemberAndType_MemberWithAttributes_ReturnsAllAttributes()
        {
            // Arrange
            var memberInfo = typeof(SampleClass).GetProperty("Name");
            var type = typeof(SampleClass);

            // Act
            var attributes = ReflectionUtil.GetAttributesOfMemberAndType(memberInfo, type);

            // Assert
            Assert.NotEmpty(attributes);
            Assert.Contains(attributes, attr => attr is SampleAttribute sampleAttr && sampleAttr.Value == "PropertyAttribute");
            Assert.Contains(attributes, attr => attr is SampleAttribute sampleAttr && sampleAttr.Value == "ClassAttribute");
        }

        /// <summary>
        /// Tests that GetAttributesOfMemberAndType throws ArgumentNullException when memberInfo is null.
        /// </summary>
        [Fact]
        public void GetAttributesOfMemberAndType_MemberInfoIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            MemberInfo memberInfo = null;
            var type = typeof(SampleClass);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ReflectionUtil.GetAttributesOfMemberAndType(memberInfo, type));
        }

        /// <summary>
        /// Tests that GetAttributesOfMemberAndType throws ArgumentNullException when type is null.
        /// </summary>
        [Fact]
        public void GetAttributesOfMemberAndType_TypeIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var memberInfo = typeof(SampleClass).GetProperty("Name");
            Type type = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ReflectionUtil.GetAttributesOfMemberAndType(memberInfo, type));
        }

        #endregion

        #region GetSingleAttributeOfMemberOrDeclaringTypeOrDefault Tests

        /// <summary>
        /// Tests that GetSingleAttributeOfMemberOrDeclaringTypeOrDefault returns the attribute when found.
        /// </summary>
        [Fact]
        public void GetSingleAttributeOfMemberOrDeclaringTypeOrDefault_AttributeExists_ReturnsAttribute()
        {
            // Arrange
            var memberInfo = typeof(SampleClass).GetProperty("Name");

            // Act
            var attribute = ReflectionUtil.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<SampleAttribute>(memberInfo);

            // Assert
            Assert.NotNull(attribute);
            Assert.Equal("PropertyAttribute", attribute.Value);
        }

        /// <summary>
        /// Tests that GetSingleAttributeOfMemberOrDeclaringTypeOrDefault returns default value when attribute not found.
        /// </summary>
        [Fact]
        public void GetSingleAttributeOfMemberOrDeclaringTypeOrDefault_AttributeNotFound_ReturnsDefault()
        {
            // Arrange
            var memberInfo = typeof(SampleClass).GetProperty("Age");
            var defaultValue = new SampleAttribute("Default");

            // Act
            var attribute = ReflectionUtil.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault(memberInfo, defaultValue);

            // Assert
            Assert.NotNull(attribute);
            Assert.Equal("ClassAttribute", attribute.Value); // Should find class attribute
        }

        /// <summary>
        /// Tests that GetSingleAttributeOfMemberOrDeclaringTypeOrDefault throws ArgumentNullException when memberInfo is null.
        /// </summary>
        [Fact]
        public void GetSingleAttributeOfMemberOrDeclaringTypeOrDefault_MemberInfoIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            MemberInfo memberInfo = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ReflectionUtil.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<SampleAttribute>(memberInfo));
        }

        #endregion

        #region GetSingleAttributeOrDefault Tests

        /// <summary>
        /// Tests that GetSingleAttributeOrDefault returns the attribute when found.
        /// </summary>
        [Fact]
        public void GetSingleAttributeOrDefault_AttributeExists_ReturnsAttribute()
        {
            // Arrange
            var memberInfo = typeof(SampleClass).GetProperty("Name");

            // Act
            var attribute = ReflectionUtil.GetSingleAttributeOrDefault<SampleAttribute>(memberInfo);

            // Assert
            Assert.NotNull(attribute);
            Assert.Equal("PropertyAttribute", attribute.Value);
        }

        /// <summary>
        /// Tests that GetSingleAttributeOrDefault returns default value when attribute not found.
        /// </summary>
        [Fact]
        public void GetSingleAttributeOrDefault_AttributeNotFound_ReturnsDefault()
        {
            // Arrange
            var memberInfo = typeof(SampleClass).GetProperty("Age");
            var defaultValue = new SampleAttribute("Default");

            // Act
            var attribute = ReflectionUtil.GetSingleAttributeOrDefault(memberInfo, defaultValue);

            // Assert
            Assert.Equal(defaultValue, attribute);
        }

        /// <summary>
        /// Tests that GetSingleAttributeOrDefault throws ArgumentNullException when memberInfo is null.
        /// </summary>
        [Fact]
        public void GetSingleAttributeOrDefault_MemberInfoIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            MemberInfo memberInfo = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ReflectionUtil.GetSingleAttributeOrDefault<SampleAttribute>(memberInfo));
        }

        #endregion

        #region GetPropertyByPath Tests

        /// <summary>
        /// Tests that GetPropertyByPath returns the correct property for a simple path.
        /// </summary>
        [Fact]
        public void GetPropertyByPath_SimplePath_ReturnsProperty()
        {
            // Arrange
            var obj = new SampleClass { Name = "Test" };
            var objectType = typeof(SampleClass);
            var propertyPath = "Name";

            // Act
            var property = ReflectionUtil.GetPropertyByPath(obj, objectType, propertyPath);

            // Assert
            Assert.NotNull(property);
            Assert.Equal("Name", property.Name);
            Assert.Equal(typeof(string), property.PropertyType);
        }

        /// <summary>
        /// Tests that GetPropertyByPath returns the correct property for a nested path.
        /// </summary>
        [Fact]
        public void GetPropertyByPath_NestedPath_ReturnsProperty()
        {
            // Arrange
            var obj = new SampleClass 
            { 
                Nested = new SampleNestedClass { NestedProperty = "NestedValue" } 
            };
            var objectType = typeof(SampleClass);
            var propertyPath = "Nested.NestedProperty";

            // Act
            var property = ReflectionUtil.GetPropertyByPath(obj, objectType, propertyPath);

            // Assert
            Assert.NotNull(property);
            Assert.Equal("NestedProperty", property.Name);
            Assert.Equal(typeof(string), property.PropertyType);
        }

        /// <summary>
        /// Tests that GetPropertyByPath throws ArgumentException for invalid property path.
        /// </summary>
        [Fact]
        public void GetPropertyByPath_InvalidPath_ThrowsArgumentException()
        {
            // Arrange
            var obj = new SampleClass();
            var objectType = typeof(SampleClass);
            var propertyPath = "InvalidProperty";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => ReflectionUtil.GetPropertyByPath(obj, objectType, propertyPath));
        }

        /// <summary>
        /// Tests that GetPropertyByPath throws ArgumentNullException when obj is null.
        /// </summary>
        [Fact]
        public void GetPropertyByPath_ObjIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            object obj = null;
            var objectType = typeof(SampleClass);
            var propertyPath = "Name";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ReflectionUtil.GetPropertyByPath(obj, objectType, propertyPath));
        }

        #endregion

        #region GetValueByPath Tests

        /// <summary>
        /// Tests that GetValueByPath returns the correct value for a simple path.
        /// </summary>
        [Fact]
        public void GetValueByPath_SimplePath_ReturnsValue()
        {
            // Arrange
            var obj = new SampleClass { Name = "TestValue" };
            var objectType = typeof(SampleClass);
            var propertyPath = "Name";

            // Act
            var value = ReflectionUtil.GetValueByPath(obj, objectType, propertyPath);

            // Assert
            Assert.Equal("TestValue", value);
        }

        /// <summary>
        /// Tests that GetValueByPath returns the correct value for a nested path.
        /// </summary>
        [Fact]
        public void GetValueByPath_NestedPath_ReturnsValue()
        {
            // Arrange
            var obj = new SampleClass 
            { 
                Nested = new SampleNestedClass { NestedProperty = "NestedValue" } 
            };
            var objectType = typeof(SampleClass);
            var propertyPath = "Nested.NestedProperty";

            // Act
            var value = ReflectionUtil.GetValueByPath(obj, objectType, propertyPath);

            // Assert
            Assert.Equal("NestedValue", value);
        }

        /// <summary>
        /// Tests that GetValueByPath throws ArgumentException for invalid property path.
        /// </summary>
        [Fact]
        public void GetValueByPath_InvalidPath_ThrowsArgumentException()
        {
            // Arrange
            var obj = new SampleClass();
            var objectType = typeof(SampleClass);
            var propertyPath = "InvalidProperty";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => ReflectionUtil.GetValueByPath(obj, objectType, propertyPath));
        }

        #endregion

        #region SetValueByPath Tests

        /// <summary>
        /// Tests that SetValueByPath sets the correct value for a simple path.
        /// </summary>
        [Fact]
        public void SetValueByPath_SimplePath_SetsValue()
        {
            // Arrange
            var obj = new SampleClass();
            var objectType = typeof(SampleClass);
            var propertyPath = "Name";
            var value = "NewValue";

            // Act
            ReflectionUtil.SetValueByPath(obj, objectType, propertyPath, value);

            // Assert
            Assert.Equal("NewValue", obj.Name);
        }

        /// <summary>
        /// Tests that SetValueByPath sets the correct value for a nested path.
        /// </summary>
        [Fact]
        public void SetValueByPath_NestedPath_SetsValue()
        {
            // Arrange
            var obj = new SampleClass 
            { 
                Nested = new SampleNestedClass() 
            };
            var objectType = typeof(SampleClass);
            var propertyPath = "Nested.NestedProperty";
            var value = "NewNestedValue";

            // Act
            ReflectionUtil.SetValueByPath(obj, objectType, propertyPath, value);

            // Assert
            Assert.Equal("NewNestedValue", obj.Nested.NestedProperty);
        }

        /// <summary>
        /// Tests that SetValueByPath throws ArgumentException for invalid property path.
        /// </summary>
        [Fact]
        public void SetValueByPath_InvalidPath_ThrowsArgumentException()
        {
            // Arrange
            var obj = new SampleClass();
            var objectType = typeof(SampleClass);
            var propertyPath = "InvalidProperty";
            var value = "SomeValue";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => ReflectionUtil.SetValueByPath(obj, objectType, propertyPath, value));
        }

        #endregion

        #region IsPropertyGetterSetterMethod Tests

        /// <summary>
        /// Tests that IsPropertyGetterSetterMethod returns true for property getter methods.
        /// </summary>
        [Fact]
        public void IsPropertyGetterSetterMethod_PropertyGetter_ReturnsTrue()
        {
            // Arrange
            var type = typeof(SampleClass);
            var method = type.GetMethod("get_Name", BindingFlags.Public | BindingFlags.Instance);

            // Act
            var result = ReflectionUtil.IsPropertyGetterSetterMethod(method, type);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Tests that IsPropertyGetterSetterMethod returns true for property setter methods.
        /// </summary>
        [Fact]
        public void IsPropertyGetterSetterMethod_PropertySetter_ReturnsTrue()
        {
            // Arrange
            var type = typeof(SampleClass);
            var method = type.GetMethod("set_Name", BindingFlags.Public | BindingFlags.Instance);

            // Act
            var result = ReflectionUtil.IsPropertyGetterSetterMethod(method, type);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Tests that IsPropertyGetterSetterMethod returns false for regular methods.
        /// </summary>
        [Fact]
        public void IsPropertyGetterSetterMethod_RegularMethod_ReturnsFalse()
        {
            // Arrange
            var type = typeof(SampleClass);
            var method = type.GetMethod("SampleMethod");

            // Act
            var result = ReflectionUtil.IsPropertyGetterSetterMethod(method, type);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Tests that IsPropertyGetterSetterMethod throws ArgumentNullException when method is null.
        /// </summary>
        [Fact]
        public void IsPropertyGetterSetterMethod_MethodIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            MethodInfo method = null;
            var type = typeof(SampleClass);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ReflectionUtil.IsPropertyGetterSetterMethod(method, type));
        }

        /// <summary>
        /// Tests that IsPropertyGetterSetterMethod throws ArgumentNullException when type is null.
        /// </summary>
        [Fact]
        public void IsPropertyGetterSetterMethod_TypeIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var type = typeof(SampleClass);
            var method = type.GetMethod("get_Name", BindingFlags.Public | BindingFlags.Instance);
            Type nullType = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ReflectionUtil.IsPropertyGetterSetterMethod(method, nullType));
        }

        /// <summary>
        /// Tests that IsPropertyGetterSetterMethod returns false for method with short name.
        /// </summary>
        [Fact]
        public void IsPropertyGetterSetterMethod_ShortMethodName_ReturnsFalse()
        {
            var type = typeof(SampleClass);
            var method = type.GetMethod("get_TestProperty");

            var result = ReflectionUtil.IsPropertyGetterSetterMethod(method, type);

            Assert.False(result);
        }

        #endregion

        #region Additional IsAssignableToGenericType Tests

        /// <summary>
        /// Tests that IsAssignableToGenericType returns true when type inherits from generic base class.
        /// </summary>
        [Fact]
        public void IsAssignableToGenericType_InheritsFromGenericBase_ReturnsTrue()
        {
            var givenType = typeof(DerivedGenericClass);
            var genericType = typeof(GenericClass<>);

            var result = ReflectionUtil.IsAssignableToGenericType(givenType, genericType);

            Assert.True(result);
        }

        public class DerivedGenericClass : GenericClass<string>
        {
        }

        #endregion

        #region Additional GetPropertyByPath Tests

        /// <summary>
        /// Tests that GetPropertyByPath handles full type name in path.
        /// </summary>
        [Fact]
        public void GetPropertyByPath_WithFullTypeNamePath_ReturnsProperty()
        {
            var obj = new SampleClass { Name = "Test" };
            var objectType = typeof(SampleClass);
            var propertyPath = $"{typeof(SampleClass).FullName}.Name";

            var property = ReflectionUtil.GetPropertyByPath(obj, objectType, propertyPath);

            Assert.NotNull(property);
            Assert.Equal("Name", property.Name);
        }

        /// <summary>
        /// Tests that GetPropertyByPath throws ArgumentNullException when objectType is null.
        /// </summary>
        [Fact]
        public void GetPropertyByPath_ObjectTypeIsNull_ThrowsArgumentNullException()
        {
            object obj = new SampleClass();
            Type objectType = null;
            var propertyPath = "Name";

            Assert.Throws<ArgumentNullException>(() => ReflectionUtil.GetPropertyByPath(obj, objectType, propertyPath));
        }

        /// <summary>
        /// Tests that GetPropertyByPath throws ArgumentNullException when propertyPath is null.
        /// </summary>
        [Fact]
        public void GetPropertyByPath_PropertyPathIsNull_ThrowsArgumentNullException()
        {
            object obj = new SampleClass();
            var objectType = typeof(SampleClass);
            string propertyPath = null;

            Assert.Throws<ArgumentNullException>(() => ReflectionUtil.GetPropertyByPath(obj, objectType, propertyPath));
        }

        #endregion

        #region Additional GetValueByPath Tests

        /// <summary>
        /// Tests that GetValueByPath handles full type name in path.
        /// </summary>
        [Fact]
        public void GetValueByPath_WithFullTypeNamePath_ReturnsValue()
        {
            var obj = new SampleClass { Name = "TestValue" };
            var objectType = typeof(SampleClass);
            var propertyPath = $"{typeof(SampleClass).FullName}.Name";

            var value = ReflectionUtil.GetValueByPath(obj, objectType, propertyPath);

            Assert.Equal("TestValue", value);
        }

        /// <summary>
        /// Tests that GetValueByPath throws ArgumentNullException when obj is null.
        /// </summary>
        [Fact]
        public void GetValueByPath_ObjIsNull_ThrowsArgumentNullException()
        {
            object obj = null;
            var objectType = typeof(SampleClass);
            var propertyPath = "Name";

            Assert.Throws<ArgumentNullException>(() => ReflectionUtil.GetValueByPath(obj, objectType, propertyPath));
        }

        /// <summary>
        /// Tests that GetValueByPath throws ArgumentNullException when objectType is null.
        /// </summary>
        [Fact]
        public void GetValueByPath_ObjectTypeIsNull_ThrowsArgumentNullException()
        {
            object obj = new SampleClass();
            Type objectType = null;
            var propertyPath = "Name";

            Assert.Throws<ArgumentNullException>(() => ReflectionUtil.GetValueByPath(obj, objectType, propertyPath));
        }

        /// <summary>
        /// Tests that GetValueByPath throws ArgumentNullException when propertyPath is null.
        /// </summary>
        [Fact]
        public void GetValueByPath_PropertyPathIsNull_ThrowsArgumentNullException()
        {
            object obj = new SampleClass();
            var objectType = typeof(SampleClass);
            string propertyPath = null;

            Assert.Throws<ArgumentNullException>(() => ReflectionUtil.GetValueByPath(obj, objectType, propertyPath));
        }

        /// <summary>
        /// Tests that GetValueByPath throws ArgumentException for invalid nested property path.
        /// </summary>
        [Fact]
        public void GetValueByPath_InvalidNestedPath_ThrowsArgumentException()
        {
            var obj = new SampleClass { Nested = new SampleNestedClass() };
            var objectType = typeof(SampleClass);
            var propertyPath = "Nested.InvalidProperty";

            Assert.Throws<ArgumentException>(() => ReflectionUtil.GetValueByPath(obj, objectType, propertyPath));
        }

        #endregion

        #region Additional SetValueByPath Tests

        /// <summary>
        /// Tests that SetValueByPath handles full type name in path.
        /// </summary>
        [Fact]
        public void SetValueByPath_WithFullTypeNamePath_SetsValue()
        {
            var obj = new SampleClass();
            var objectType = typeof(SampleClass);
            var propertyPath = $"{typeof(SampleClass).FullName}.Name";
            var value = "NewValue";

            ReflectionUtil.SetValueByPath(obj, objectType, propertyPath, value);

            Assert.Equal("NewValue", obj.Name);
        }

        /// <summary>
        /// Tests that SetValueByPath throws ArgumentNullException when obj is null.
        /// </summary>
        [Fact]
        public void SetValueByPath_ObjIsNull_ThrowsArgumentNullException()
        {
            object obj = null;
            var objectType = typeof(SampleClass);
            var propertyPath = "Name";
            var value = "NewValue";

            Assert.Throws<ArgumentNullException>(() => ReflectionUtil.SetValueByPath(obj, objectType, propertyPath, value));
        }

        /// <summary>
        /// Tests that SetValueByPath throws ArgumentNullException when objectType is null.
        /// </summary>
        [Fact]
        public void SetValueByPath_ObjectTypeIsNull_ThrowsArgumentNullException()
        {
            object obj = new SampleClass();
            Type objectType = null;
            var propertyPath = "Name";
            var value = "NewValue";

            Assert.Throws<ArgumentNullException>(() => ReflectionUtil.SetValueByPath(obj, objectType, propertyPath, value));
        }

        /// <summary>
        /// Tests that SetValueByPath throws ArgumentNullException when propertyPath is null.
        /// </summary>
        [Fact]
        public void SetValueByPath_PropertyPathIsNull_ThrowsArgumentNullException()
        {
            object obj = new SampleClass();
            var objectType = typeof(SampleClass);
            string propertyPath = null;
            var value = "NewValue";

            Assert.Throws<ArgumentNullException>(() => ReflectionUtil.SetValueByPath(obj, objectType, propertyPath, value));
        }

        /// <summary>
        /// Tests that SetValueByPath throws ArgumentException for invalid nested property path.
        /// </summary>
        [Fact]
        public void SetValueByPath_InvalidNestedPath_ThrowsArgumentException()
        {
            var obj = new SampleClass { Nested = new SampleNestedClass() };
            var objectType = typeof(SampleClass);
            var propertyPath = "Nested.InvalidProperty";
            var value = "SomeValue";

            Assert.Throws<ArgumentException>(() => ReflectionUtil.SetValueByPath(obj, objectType, propertyPath, value));
        }

        #endregion

        #region GetAttributesOfMemberAndDeclaringType<TAttribute> Tests

        /// <summary>
        /// Tests that GetAttributesOfMemberAndDeclaringType generic returns typed attributes.
        /// </summary>
        [Fact]
        public void GetAttributesOfMemberAndDeclaringType_Generic_ReturnsTypedAttributes()
        {
            var memberInfo = typeof(SampleClass).GetProperty("Name");

            var attributes = ReflectionUtil.GetAttributesOfMemberAndDeclaringType<SampleAttribute>(memberInfo);

            Assert.NotEmpty(attributes);
            Assert.Contains(attributes, attr => attr.Value == "PropertyAttribute");
            Assert.Contains(attributes, attr => attr.Value == "ClassAttribute");
        }

        /// <summary>
        /// Tests that GetAttributesOfMemberAndDeclaringType generic throws ArgumentNullException when memberInfo is null.
        /// </summary>
        [Fact]
        public void GetAttributesOfMemberAndDeclaringType_Generic_MemberInfoIsNull_ThrowsArgumentNullException()
        {
            MemberInfo memberInfo = null;

            Assert.Throws<ArgumentNullException>(() => ReflectionUtil.GetAttributesOfMemberAndDeclaringType<SampleAttribute>(memberInfo));
        }

        #endregion

        #region GetAttributesOfMemberAndType<TAttribute> Tests

        /// <summary>
        /// Tests that GetAttributesOfMemberAndType generic returns typed attributes.
        /// </summary>
        [Fact]
        public void GetAttributesOfMemberAndType_Generic_ReturnsTypedAttributes()
        {
            var memberInfo = typeof(SampleClass).GetProperty("Name");
            var type = typeof(SampleClass);

            var attributes = ReflectionUtil.GetAttributesOfMemberAndType<SampleAttribute>(memberInfo, type);

            Assert.NotEmpty(attributes);
            Assert.Contains(attributes, attr => attr.Value == "PropertyAttribute");
            Assert.Contains(attributes, attr => attr.Value == "ClassAttribute");
        }

        /// <summary>
        /// Tests that GetAttributesOfMemberAndType generic throws ArgumentNullException when memberInfo is null.
        /// </summary>
        [Fact]
        public void GetAttributesOfMemberAndType_Generic_MemberInfoIsNull_ThrowsArgumentNullException()
        {
            MemberInfo memberInfo = null;
            var type = typeof(SampleClass);

            Assert.Throws<ArgumentNullException>(() => ReflectionUtil.GetAttributesOfMemberAndType<SampleAttribute>(memberInfo, type));
        }

        /// <summary>
        /// Tests that GetAttributesOfMemberAndType generic throws ArgumentNullException when type is null.
        /// </summary>
        [Fact]
        public void GetAttributesOfMemberAndType_Generic_TypeIsNull_ThrowsArgumentNullException()
        {
            var memberInfo = typeof(SampleClass).GetProperty("Name");
            Type type = null;

            Assert.Throws<ArgumentNullException>(() => ReflectionUtil.GetAttributesOfMemberAndType<SampleAttribute>(memberInfo, type));
        }

        #endregion
    }
}