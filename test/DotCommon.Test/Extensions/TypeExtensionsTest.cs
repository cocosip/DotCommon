using System;
using Xunit;

namespace DotCommon.Test.Extensions
{
    public class TypeExtensionsTest
    {
        [Fact]
        public void GetAssembly_Test()
        {
            var assembly = typeof(TypeExtensionsTest).GetAssembly();
            Assert.Equal("DotCommon.Test", assembly.GetName().Name);
        }

        [Fact]
        public void GetMethod_Test()
        {
            var methodInfo = typeof(TypeExtensionsTest).GetMethod("Method1", 1, 0);
            Assert.Equal("Method1", methodInfo.Name);
        }

        [Fact]
        public void GetMethod_WithGenericArguments_ShouldReturnMethod()
        {
            var methodInfo = typeof(TypeExtensionsTest).GetMethod("GenericMethod", 0, 1);
            Assert.True(methodInfo.IsGenericMethod);
        }

        [Fact]
        public void GetFullNameWithAssemblyName_Test()
        {
            var fullName = typeof(TypeExtensionsTest).GetFullNameWithAssemblyName();
            Assert.Contains("DotCommon.Test", fullName);
        }

        [Fact]
        public void IsAssignableTo_WithValidTarget_ShouldReturnTrue()
        {
            Assert.True(typeof(string).IsAssignableTo<object>());
        }

        [Fact]
        public void IsAssignableTo_WithInvalidTarget_ShouldReturnFalse()
        {
            Assert.False(typeof(string).IsAssignableTo<int>());
        }

        [Fact]
        public void IsAssignableTo_WithType_ShouldWork()
        {
            Assert.True(typeof(string).IsAssignableTo(typeof(object)));
        }

        [Fact]
        public void GetBaseClasses_ShouldReturnBaseTypes()
        {
            var bases = typeof(DerivedClass).GetBaseClasses();
            Assert.Contains(typeof(BaseClass), bases);
        }

        [Fact]
        public void GetBaseClasses_WithIncludeObjectFalse_ShouldExcludeObject()
        {
            var bases = typeof(DerivedClass).GetBaseClasses(includeObject: false);
            Assert.DoesNotContain(typeof(object), bases);
        }

        [Fact]
        public void GetBaseClasses_WithStoppingType_ShouldStop()
        {
            var bases = typeof(DerivedClass).GetBaseClasses(typeof(BaseClass));
            Assert.DoesNotContain(typeof(object), bases);
        }

        public string Method1(int id)
        {
            return $"{id}-Name";
        }

        public T GenericMethod<T>() => default;

        private class BaseClass { }
        private class DerivedClass : BaseClass { }
    }
}
