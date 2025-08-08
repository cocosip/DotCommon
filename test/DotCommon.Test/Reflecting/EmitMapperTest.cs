using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DotCommon.Reflecting;
using Xunit;

namespace DotCommon.Test.Reflecting
{
    /// <summary>
    /// Unit tests for <see cref="MemberInfoExtensions"/> class.
    /// </summary>
    public class EmitMapperTest
    {
        #region Test Classes and Attributes

        [AttributeUsage(AttributeTargets.All)]
        public class TestAttribute : Attribute
        {
            public string Value { get; set; }

            public TestAttribute(string value = "")
            {
                Value = value;
            }
        }

        [AttributeUsage(AttributeTargets.All)]
        public class AnotherTestAttribute : Attribute
        {
        }

        [Test("BaseClass")]
        public class BaseTestClass
        {
            [Test("BaseProperty")]
            public virtual string BaseProperty { get; set; } = "";

            [Test("BaseMethod")]
            public virtual void BaseMethod() { }

            [Test("BaseField")]
            public string BaseField = "";
        }

        [Test("DerivedClass")]
        public class DerivedTestClass : BaseTestClass
        {
            [Test("DerivedProperty")]
            public string DerivedProperty { get; set; } = "";

            [Test("DerivedMethod")]
            public override void BaseMethod() { }

            [Test("DerivedField")]
            public string DerivedField = "";

            public string AutoProperty { get; set; } = "";

            public string ReadOnlyProperty { get; } = "";

            private string _backingField = "";
            public string ManualProperty
            {
                get => _backingField;
                set => _backingField = value;
            }

            public async Task AsyncMethod()
            {
                await Task.Delay(1);
            }

            public Task TaskMethod()
            {
                return Task.CompletedTask;
            }

            public Task<string> TaskWithResultMethod()
            {
                return Task.FromResult("test");
            }

            public void SyncMethod() { }

            public void MethodWithParameters(int param1, string param2) { }
        }

        public interface ITestInterface
        {
            void InterfaceMethod();
        }

        public interface IGenericInterface<T>
        {
            T GenericMethod(T value);
        }

        public class ImplementingClass : ITestInterface, IGenericInterface<string>
        {
            public void InterfaceMethod() { }
            public string GenericMethod(string value) => value;
        }

        public struct TestStruct
        {
            public int Value { get; set; }
        }

        #endregion

        #region Attribute Extensions Tests

        [Fact]
        public void GetSingleAttributeOrNull_WithValidAttribute_ShouldReturnAttribute()
        {
            // Arrange
            var type = typeof(DerivedTestClass);

            // Act
            var attribute = type.GetSingleAttributeOrNull<TestAttribute>();

            // Assert
            Assert.NotNull(attribute);
            Assert.Equal("DerivedClass", attribute.Value);
        }

        [Fact]
        public void GetSingleAttributeOrNull_WithoutAttribute_ShouldReturnNull()
        {
            // Arrange
            var type = typeof(string);

            // Act
            var attribute = type.GetSingleAttributeOrNull<TestAttribute>();

            // Assert
            Assert.Null(attribute);
        }

        [Fact]
        public void GetSingleAttributeOrNull_WithNullMemberInfo_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                ((MemberInfo)null!).GetSingleAttributeOrNull<TestAttribute>());
        }

        [Fact]
        public void GetCustomAttributes_WithValidAttributes_ShouldReturnAllAttributes()
        {
            // Arrange
            var type = typeof(DerivedTestClass);

            // Act
            var attributes = type.GetCustomAttributes<TestAttribute>();

            // Assert
            Assert.NotEmpty(attributes);
            Assert.Contains(attributes, a => a.Value == "DerivedClass");
        }

        [Fact]
        public void GetCustomAttributes_WithNullMemberInfo_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                ((MemberInfo)null!).GetCustomAttributes<TestAttribute>());
        }

        [Fact]
        public void HasCustomAttribute_WithValidAttribute_ShouldReturnTrue()
        {
            // Arrange
            var type = typeof(DerivedTestClass);

            // Act
            var hasAttribute = type.HasCustomAttribute<TestAttribute>();

            // Assert
            Assert.True(hasAttribute);
        }

        [Fact]
        public void HasCustomAttribute_WithoutAttribute_ShouldReturnFalse()
        {
            // Arrange
            var type = typeof(string);

            // Act
            var hasAttribute = type.HasCustomAttribute<TestAttribute>();

            // Assert
            Assert.False(hasAttribute);
        }

        [Fact]
        public void HasCustomAttribute_WithNullMemberInfo_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                ((MemberInfo)null!).HasCustomAttribute<TestAttribute>());
        }

        [Fact]
        public void GetSingleAttributeOfTypeOrBaseTypesOrNull_WithAttributeOnBaseType_ShouldReturnAttribute()
        {
            // Arrange
            var type = typeof(DerivedTestClass);

            // Act
            var attribute = type.GetSingleAttributeOfTypeOrBaseTypesOrNull<TestAttribute>();

            // Assert
            Assert.NotNull(attribute);
        }

        [Fact]
        public void GetSingleAttributeOfTypeOrBaseTypesOrNull_WithNullType_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                ((Type)null!).GetSingleAttributeOfTypeOrBaseTypesOrNull<TestAttribute>());
        }

        #endregion

        #region Type Extensions Tests

        [Fact]
        public void IsNullableType_WithNullableType_ShouldReturnTrue()
        {
            // Arrange
            var type = typeof(int?);

            // Act
            var isNullable = type.IsNullableType();

            // Assert
            Assert.True(isNullable);
        }

        [Fact]
        public void IsNullableType_WithNonNullableType_ShouldReturnFalse()
        {
            // Arrange
            var type = typeof(int);

            // Act
            var isNullable = type.IsNullableType();

            // Assert
            Assert.False(isNullable);
        }

        [Fact]
        public void IsNullableType_WithNullType_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                ((Type)null!).IsNullableType());
        }

        [Fact]
        public void GetNonNullableType_WithNullableType_ShouldReturnUnderlyingType()
        {
            // Arrange
            var type = typeof(int?);

            // Act
            var nonNullableType = type.GetNonNullableType();

            // Assert
            Assert.Equal(typeof(int), nonNullableType);
        }

        [Fact]
        public void GetNonNullableType_WithNonNullableType_ShouldReturnSameType()
        {
            // Arrange
            var type = typeof(int);

            // Act
            var nonNullableType = type.GetNonNullableType();

            // Assert
            Assert.Equal(typeof(int), nonNullableType);
        }

        [Fact]
        public void GetNonNullableType_WithNullType_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                ((Type)null!).GetNonNullableType());
        }

        [Theory]
        [InlineData(typeof(byte), true)]
        [InlineData(typeof(sbyte), true)]
        [InlineData(typeof(short), true)]
        [InlineData(typeof(ushort), true)]
        [InlineData(typeof(int), true)]
        [InlineData(typeof(uint), true)]
        [InlineData(typeof(long), true)]
        [InlineData(typeof(ulong), true)]
        [InlineData(typeof(float), true)]
        [InlineData(typeof(double), true)]
        [InlineData(typeof(decimal), true)]
        [InlineData(typeof(string), false)]
        [InlineData(typeof(bool), false)]
        [InlineData(typeof(DateTime), false)]
        public void IsNumericType_ShouldReturnCorrectResult(Type type, bool expected)
        {
            // Act
            var isNumeric = type.IsNumericType();

            // Assert
            Assert.Equal(expected, isNumeric);
        }

        [Fact]
        public void IsNumericType_WithNullableNumericType_ShouldReturnTrue()
        {
            // Arrange
            var type = typeof(int?);

            // Act
            var isNumeric = type.IsNumericType();

            // Assert
            Assert.True(isNumeric);
        }

        [Theory]
        [InlineData(typeof(int), true)]
        [InlineData(typeof(bool), true)]
        [InlineData(typeof(char), true)]
        [InlineData(typeof(string), true)]
        [InlineData(typeof(decimal), true)]
        [InlineData(typeof(DateTime), false)]
        [InlineData(typeof(object), false)]
        public void IsPrimitiveOrString_ShouldReturnCorrectResult(Type type, bool expected)
        {
            // Act
            var isPrimitiveOrString = type.IsPrimitiveOrString();

            // Assert
            Assert.Equal(expected, isPrimitiveOrString);
        }

        [Fact]
        public void GetAllInterfaces_ShouldReturnAllInterfaces()
        {
            // Arrange
            var type = typeof(ImplementingClass);

            // Act
            var interfaces = type.GetAllInterfaces();

            // Assert
            Assert.Contains(typeof(ITestInterface), interfaces);
            Assert.Contains(typeof(IGenericInterface<string>), interfaces);
        }

        [Fact]
        public void ImplementsInterface_WithImplementedInterface_ShouldReturnTrue()
        {
            // Arrange
            var type = typeof(ImplementingClass);
            var interfaceType = typeof(ITestInterface);

            // Act
            var implements = type.ImplementsInterface(interfaceType);

            // Assert
            Assert.True(implements);
        }

        [Fact]
        public void ImplementsInterface_WithNotImplementedInterface_ShouldReturnFalse()
        {
            // Arrange
            var type = typeof(string);
            var interfaceType = typeof(ITestInterface);

            // Act
            var implements = type.ImplementsInterface(interfaceType);

            // Assert
            Assert.False(implements);
        }

        [Fact]
        public void ImplementsGenericInterface_WithImplementedGenericInterface_ShouldReturnTrue()
        {
            // Arrange
            var type = typeof(ImplementingClass);
            var genericInterfaceType = typeof(IGenericInterface<>);

            // Act
            var implements = type.ImplementsGenericInterface(genericInterfaceType);

            // Assert
            Assert.True(implements);
        }

        #endregion

        #region MethodInfo Extensions Tests

        [Fact]
        public void HasAsyncStateMachine_WithAsyncMethod_ShouldReturnTrue()
        {
            // Arrange
            var method = typeof(DerivedTestClass).GetMethod(nameof(DerivedTestClass.AsyncMethod))!;

            // Act
            var hasAsyncStateMachine = method.HasAsyncStateMachine();

            // Assert
            Assert.True(hasAsyncStateMachine);
        }

        [Fact]
        public void HasAsyncStateMachine_WithSyncMethod_ShouldReturnFalse()
        {
            // Arrange
            var method = typeof(DerivedTestClass).GetMethod(nameof(DerivedTestClass.SyncMethod))!;

            // Act
            var hasAsyncStateMachine = method.HasAsyncStateMachine();

            // Assert
            Assert.False(hasAsyncStateMachine);
        }

        [Fact]
        public void ReturnsTask_WithTaskMethod_ShouldReturnTrue()
        {
            // Arrange
            var method = typeof(DerivedTestClass).GetMethod(nameof(DerivedTestClass.TaskMethod))!;

            // Act
            var returnsTask = method.ReturnsTask();

            // Assert
            Assert.True(returnsTask);
        }

        [Fact]
        public void ReturnsTask_WithTaskWithResultMethod_ShouldReturnTrue()
        {
            // Arrange
            var method = typeof(DerivedTestClass).GetMethod(nameof(DerivedTestClass.TaskWithResultMethod))!;

            // Act
            var returnsTask = method.ReturnsTask();

            // Assert
            Assert.True(returnsTask);
        }

        [Fact]
        public void ReturnsTask_WithSyncMethod_ShouldReturnFalse()
        {
            // Arrange
            var method = typeof(DerivedTestClass).GetMethod(nameof(DerivedTestClass.SyncMethod))!;

            // Act
            var returnsTask = method.ReturnsTask();

            // Assert
            Assert.False(returnsTask);
        }

        [Fact]
        public void GetParameterTypes_ShouldReturnCorrectTypes()
        {
            // Arrange
            var method = typeof(DerivedTestClass).GetMethod(nameof(DerivedTestClass.MethodWithParameters))!;

            // Act
            var parameterTypes = method.GetParameterTypes();

            // Assert
            Assert.Equal(2, parameterTypes.Length);
            Assert.Equal(typeof(int), parameterTypes[0]);
            Assert.Equal(typeof(string), parameterTypes[1]);
        }

        #endregion

        #region PropertyInfo Extensions Tests

        [Fact]
        public void IsAutoProperty_WithAutoProperty_ShouldReturnTrue()
        {
            // Arrange
            var property = typeof(DerivedTestClass).GetProperty(nameof(DerivedTestClass.AutoProperty))!;

            // Act
            var isAuto = property.IsAutoProperty();

            // Assert
            Assert.True(isAuto);
        }

        [Fact]
        public void IsAutoProperty_WithManualProperty_ShouldReturnFalse()
        {
            // Arrange
            var property = typeof(DerivedTestClass).GetProperty(nameof(DerivedTestClass.ManualProperty))!;

            // Act
            var isAuto = property.IsAutoProperty();

            // Assert
            Assert.False(isAuto);
        }

        [Fact]
        public void IsReadWrite_WithReadWriteProperty_ShouldReturnTrue()
        {
            // Arrange
            var property = typeof(DerivedTestClass).GetProperty(nameof(DerivedTestClass.AutoProperty))!;

            // Act
            var isReadWrite = property.IsReadWrite();

            // Assert
            Assert.True(isReadWrite);
        }

        [Fact]
        public void IsReadWrite_WithReadOnlyProperty_ShouldReturnFalse()
        {
            // Arrange
            var property = typeof(DerivedTestClass).GetProperty(nameof(DerivedTestClass.ReadOnlyProperty))!;

            // Act
            var isReadWrite = property.IsReadWrite();

            // Assert
            Assert.False(isReadWrite);
        }

        #endregion

        #region FieldInfo Extensions Tests

        [Fact]
        public void IsBackingField_WithBackingField_ShouldReturnTrue()
        {
            // Arrange
            var fields = typeof(DerivedTestClass).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            var backingField = fields.FirstOrDefault(f => f.Name.Contains("AutoProperty"));

            // Act & Assert
            if (backingField != null)
            {
                var isBackingField = backingField.IsBackingField();
                Assert.True(isBackingField);
            }
        }

        [Fact]
        public void IsBackingField_WithRegularField_ShouldReturnFalse()
        {
            // Arrange
            var field = typeof(DerivedTestClass).GetField(nameof(DerivedTestClass.DerivedField))!;

            // Act
            var isBackingField = field.IsBackingField();

            // Assert
            Assert.False(isBackingField);
        }

        [Fact]
        public void GetPropertyNameFromBackingField_WithBackingField_ShouldReturnPropertyName()
        {
            // Arrange
            var fields = typeof(DerivedTestClass).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            var backingField = fields.FirstOrDefault(f => f.Name.Contains("AutoProperty"));

            // Act & Assert
            if (backingField != null)
            {
                var propertyName = backingField.GetPropertyNameFromBackingField();
                Assert.Equal("AutoProperty", propertyName);
            }
        }

        [Fact]
        public void GetPropertyNameFromBackingField_WithRegularField_ShouldReturnNull()
        {
            // Arrange
            var field = typeof(DerivedTestClass).GetField(nameof(DerivedTestClass.DerivedField))!;

            // Act
            var propertyName = field.GetPropertyNameFromBackingField();

            // Assert
            Assert.Null(propertyName);
        }

        #endregion

        #region Assembly Extensions Tests

        [Fact]
        public void GetTypesImplementing_ShouldReturnImplementingTypes()
        {
            // Arrange
            var assembly = Assembly.GetExecutingAssembly();
            var interfaceType = typeof(ITestInterface);

            // Act
            var types = assembly.GetTypesImplementing(interfaceType).ToArray();

            // Assert
            Assert.Contains(typeof(ImplementingClass), types);
        }

        [Fact]
        public void GetTypesWithAttribute_ShouldReturnTypesWithAttribute()
        {
            // Arrange
            var assembly = Assembly.GetExecutingAssembly();

            // Act
            var types = assembly.GetTypesWithAttribute<TestAttribute>().ToArray();

            // Assert
            Assert.Contains(typeof(BaseTestClass), types);
            Assert.Contains(typeof(DerivedTestClass), types);
        }

        [Fact]
        public void GetTypesImplementing_WithNullAssembly_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                ((Assembly)null!).GetTypesImplementing(typeof(ITestInterface)));
        }

        [Fact]
        public void GetTypesWithAttribute_WithNullAssembly_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                ((Assembly)null!).GetTypesWithAttribute<TestAttribute>());
        }

        #endregion

        #region Null Parameter Tests

        [Fact]
        public void TypeExtensions_WithNullType_ShouldThrowArgumentNullException()
        {
            Type nullType = null!;

            Assert.Throws<ArgumentNullException>(() => nullType.GetAllInterfaces());
            Assert.Throws<ArgumentNullException>(() => nullType.ImplementsInterface(typeof(ITestInterface)));
            Assert.Throws<ArgumentNullException>(() => nullType.ImplementsGenericInterface(typeof(IGenericInterface<>)));
            Assert.Throws<ArgumentNullException>(() => nullType.IsPrimitiveOrString());
            Assert.Throws<ArgumentNullException>(() => nullType.IsNumericType());
        }

        [Fact]
        public void MethodInfoExtensions_WithNullMethod_ShouldThrowArgumentNullException()
        {
            MethodInfo nullMethod = null!;

            Assert.Throws<ArgumentNullException>(() => nullMethod.HasAsyncStateMachine());
            Assert.Throws<ArgumentNullException>(() => nullMethod.ReturnsTask());
            Assert.Throws<ArgumentNullException>(() => nullMethod.GetParameterTypes());
        }

        [Fact]
        public void PropertyInfoExtensions_WithNullProperty_ShouldThrowArgumentNullException()
        {
            PropertyInfo nullProperty = null!;

            Assert.Throws<ArgumentNullException>(() => nullProperty.IsAutoProperty());
            Assert.Throws<ArgumentNullException>(() => nullProperty.IsReadWrite());
        }

        [Fact]
        public void FieldInfoExtensions_WithNullField_ShouldThrowArgumentNullException()
        {
            FieldInfo nullField = null!;

            Assert.Throws<ArgumentNullException>(() => nullField.IsBackingField());
            Assert.Throws<ArgumentNullException>(() => nullField.GetPropertyNameFromBackingField());
        }

        #endregion
    }
}