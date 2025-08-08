using System;
using System.Reflection;
using System.Threading.Tasks;
using DotCommon.Reflecting;
using Xunit;

namespace DotCommon.Test.Reflecting
{
    /// <summary>
    /// Unit tests for TypeUtil. "TypeUtil 单元测试"
    /// </summary>
    public class TypeUtilTest
    {
        [Fact]
        public void IsFunc_ObjectIsFunc_ReturnsTrue()
        {
            Func<int> func = () => 1;
            Assert.True(TypeUtil.IsFunc(func));
        }

        [Fact]
        public void IsFunc_ObjectIsNotFunc_ReturnsFalse()
        {
            Action act = () => { };
            Assert.False(TypeUtil.IsFunc(act));
            Assert.False(TypeUtil.IsFunc(null));
        }

        [Fact]
        public void IsFuncTReturn_ObjectIsFuncOfT_ReturnsTrue()
        {
            Func<string> func = () => "test";
            Assert.True(TypeUtil.IsFunc<string>(func));
        }

        [Fact]
        public void IsFuncTReturn_ObjectIsNotFuncOfT_ReturnsFalse()
        {
            Func<int> func = () => 1;
            Assert.False(TypeUtil.IsFunc<string>(func));
            Assert.False(TypeUtil.IsFunc<string>(null));
        }

        [Fact]
        public void IsPrimitiveExtended_PrimitiveTypes_ReturnsTrue()
        {
            Assert.True(TypeUtil.IsPrimitiveExtended(typeof(int)));
            Assert.True(TypeUtil.IsPrimitiveExtended(typeof(string)));
            Assert.True(TypeUtil.IsPrimitiveExtended(typeof(decimal)));
            Assert.True(TypeUtil.IsPrimitiveExtended(typeof(DateTime)));
            Assert.True(TypeUtil.IsPrimitiveExtended(typeof(Guid)));
        }

        [Fact]
        public void IsPrimitiveExtended_NullablePrimitive_ReturnsTrue()
        {
            Assert.True(TypeUtil.IsPrimitiveExtended(typeof(int?)));
            Assert.True(TypeUtil.IsPrimitiveExtended(typeof(DateTime?)));
        }

        [Fact]
        public void IsPrimitiveExtended_Enum_ReturnsTrueIfIncludeEnums()
        {
            Assert.True(TypeUtil.IsPrimitiveExtended(typeof(AttributeTargets), true, true));
            Assert.False(TypeUtil.IsPrimitiveExtended(typeof(AttributeTargets), true, false));
        }

        [Fact]
        public void IsPrimitiveExtended_NonPrimitive_ReturnsFalse()
        {
            Assert.False(TypeUtil.IsPrimitiveExtended(typeof(TypeUtilTest)));
        }

        [Fact]
        public void IsPrimitiveExtended_TypeIsNull_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => TypeUtil.IsPrimitiveExtended(null));
        }

        [Fact]
        public void GetFirstGenericArgumentIfNullable_NullableType_ReturnsUnderlyingType()
        {
            var result = TypeUtil.GetFirstGenericArgumentIfNullable(typeof(int?));
            Assert.Equal(typeof(int), result);
        }

        [Fact]
        public void GetFirstGenericArgumentIfNullable_NonNullableType_ReturnsTypeItself()
        {
            var result = TypeUtil.GetFirstGenericArgumentIfNullable(typeof(int));
            Assert.Equal(typeof(int), result);
        }

        [Fact]
        public void IsAsync_MethodInfo_ReturnsTrueForAsync()
        {
            var method = typeof(TypeUtilTest).GetMethod(nameof(AsyncMethod), BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.True(method.IsAsync());
        }

        [Fact]
        public void IsAsync_MethodInfo_ReturnsFalseForSync()
        {
            var method = typeof(TypeUtilTest).GetMethod(nameof(SyncMethod), BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.False(method.IsAsync());
        }

        [Fact]
        public void IsAsync_MethodIsNull_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => TypeUtil.IsAsync(null));
        }

        [Fact]
        public void IsTaskOrTaskOfT_TypeIsTask_ReturnsTrue()
        {
            Assert.True(TypeUtil.IsTaskOrTaskOfT(typeof(Task)));
            Assert.True(TypeUtil.IsTaskOrTaskOfT(typeof(Task<int>)));
        }

        [Fact]
        public void IsTaskOrTaskOfT_TypeIsNotTask_ReturnsFalse()
        {
            Assert.False(TypeUtil.IsTaskOrTaskOfT(typeof(int)));
        }

        [Fact]
        public void IsTaskOrTaskOfT_TypeIsNull_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => TypeUtil.IsTaskOrTaskOfT(null));
        }

        private async Task AsyncMethod()
        {
            await Task.Delay(1);
        }

        private void SyncMethod()
        {
        }
    }
}