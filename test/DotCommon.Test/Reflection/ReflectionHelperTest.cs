using System;
using System.Collections.Generic;
using System.Reflection;
using DotCommon.Reflection;
using Xunit;

namespace DotCommon.Test.Reflection
{
    public class ReflectionHelperTest
    {
        [Fact]
        public void IsAssignableToGenericType_DirectGeneric_ShouldReturnTrue()
        {
            var result = ReflectionHelper.IsAssignableToGenericType(
                typeof(List<int>),
                typeof(List<>)
            );
            Assert.True(result);
        }

        [Fact]
        public void IsAssignableToGenericType_InterfaceGeneric_ShouldReturnTrue()
        {
            var result = ReflectionHelper.IsAssignableToGenericType(
                typeof(List<int>),
                typeof(IEnumerable<>)
            );
            Assert.True(result);
        }

        [Fact]
        public void IsAssignableToGenericType_NonGeneric_ShouldReturnFalse()
        {
            var result = ReflectionHelper.IsAssignableToGenericType(
                typeof(string),
                typeof(List<>)
            );
            Assert.False(result);
        }

        [Fact]
        public void IsAssignableToGenericType_BaseClassGeneric_ShouldReturnTrue()
        {
            var result = ReflectionHelper.IsAssignableToGenericType(
                typeof(DerivedGenericClass),
                typeof(BaseGenericClass<>)
            );
            Assert.True(result);
        }

        [Fact]
        public void GetImplementedGenericTypes_ShouldReturnAllImplementedTypes()
        {
            var result = ReflectionHelper.GetImplementedGenericTypes(
                typeof(TestClassWithMultipleGenerics),
                typeof(IEnumerable<>)
            );
            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetImplementedGenericTypes_DirectGeneric_ShouldReturn()
        {
            var result = ReflectionHelper.GetImplementedGenericTypes(
                typeof(List<int>),
                typeof(List<>)
            );
            Assert.Single(result);
            Assert.Equal(typeof(List<int>), result[0]);
        }

        [Fact]
        public void GetSingleAttributeOrDefault_WithAttribute_ShouldReturn()
        {
            var memberInfo = typeof(TestClassWithAttribute).GetProperty("TestProperty")!;
            var result = ReflectionHelper.GetSingleAttributeOrDefault<TestAttribute>(memberInfo);
            Assert.NotNull(result);
            Assert.Equal("TestValue", result.Value);
        }

        [Fact]
        public void GetSingleAttributeOrDefault_WithoutAttribute_ShouldReturnDefault()
        {
            var memberInfo = typeof(TestClassWithoutAttribute).GetProperty("TestProperty")!;
            var result = ReflectionHelper.GetSingleAttributeOrDefault<TestAttribute>(memberInfo);
            Assert.Null(result);
        }

        [Fact]
        public void GetSingleAttributeOfMemberOrDeclaringTypeOrDefault_OnMember_ShouldReturn()
        {
            var memberInfo = typeof(TestClassWithAttribute).GetProperty("TestProperty")!;
            var result = ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<TestAttribute>(memberInfo);
            Assert.NotNull(result);
        }

        [Fact]
        public void GetSingleAttributeOfMemberOrDeclaringTypeOrDefault_OnDeclaringType_ShouldReturn()
        {
            var memberInfo = typeof(TestClassWithClassAttribute).GetProperty("TestProperty")!;
            var result = ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<ClassTestAttribute>(memberInfo);
            Assert.NotNull(result);
        }

        [Fact]
        public void GetAttributesOfMemberOrDeclaringType_ShouldReturnAll()
        {
            var memberInfo = typeof(TestClassWithAttribute).GetProperty("TestProperty")!;
            var result = ReflectionHelper.GetAttributesOfMemberOrDeclaringType<TestAttribute>(memberInfo);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetValueByPath_SimpleProperty_ShouldReturn()
        {
            var obj = new TestClass { Name = "Test" };
            var result = ReflectionHelper.GetValueByPath(obj, typeof(TestClass), "Name");
            Assert.Equal("Test", result);
        }

        [Fact]
        public void GetValueByPath_NestedProperty_ShouldReturn()
        {
            var obj = new TestClassWithNested { Nested = new TestClass { Name = "NestedValue" } };
            var result = ReflectionHelper.GetValueByPath(obj, typeof(TestClassWithNested), "Nested.Name");
            Assert.Equal("NestedValue", result);
        }

        [Fact]
        public void GetValueByPath_WithFullPath_ShouldReturn()
        {
            var obj = new TestClass { Name = "Test" };
            var result = ReflectionHelper.GetValueByPath(obj, typeof(TestClass), "DotCommon.Test.Reflection.ReflectionHelperTest+TestClass.Name");
            Assert.Equal("Test", result);
        }

        [Fact]
        public void GetValueByPath_PropertyNotFound_ShouldReturnNull()
        {
            var obj = new TestClass { Name = "Test" };
            var result = ReflectionHelper.GetValueByPath(obj, typeof(TestClass), "NonExistent");
            Assert.Null(result);
        }

        [Fact]
        public void GetPublicConstantsRecursively_ShouldReturnConstants()
        {
            var result = ReflectionHelper.GetPublicConstantsRecursively(typeof(TestClassWithConstants));
            Assert.Contains("CONSTANT_VALUE", result);
        }

        [Fact]
        public void GetPublicConstantsRecursively_WithNestedType_ShouldReturnAll()
        {
            var result = ReflectionHelper.GetPublicConstantsRecursively(typeof(TestClassWithConstants));
            Assert.Contains("NESTED_CONSTANT", result);
        }

        #region Test Classes

        private class TestClass
        {
            public string Name { get; set; } = "";
        }

        private class TestClassWithNested
        {
            public TestClass Nested { get; set; } = new();
        }

        private class BaseGenericClass<T> { }

        private class DerivedGenericClass : BaseGenericClass<int> { }

        private class TestClassWithMultipleGenerics : List<int>
        {
        }

        [AttributeUsage(AttributeTargets.Property)]
        private class TestAttribute : Attribute
        {
            public string Value { get; set; } = "";
        }

        [AttributeUsage(AttributeTargets.Class)]
        private class ClassTestAttribute : Attribute
        {
            public string Value { get; set; } = "";
        }

        private class TestClassWithAttribute
        {
            [Test(Value = "TestValue")]
            public string TestProperty { get; set; } = "";
        }

        [ClassTest(Value = "ClassValue")]
        private class TestClassWithClassAttribute
        {
            public string TestProperty { get; set; } = "";
        }

        private class TestClassWithoutAttribute
        {
            public string TestProperty { get; set; } = "";
        }

        private class TestClassWithConstants
        {
            public const string CONSTANT_VALUE = "CONSTANT_VALUE";

            public class NestedConstants
            {
                public const string NESTED_CONSTANT = "NESTED_CONSTANT";
            }
        }

        #endregion
    }
}