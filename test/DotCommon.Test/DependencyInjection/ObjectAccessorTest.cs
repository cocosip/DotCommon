using DotCommon.DependencyInjection;
using Xunit;

namespace DotCommon.Test.DependencyInjection
{
    public class ObjectAccessorTest
    {
        [Fact]
        public void Constructor_Default_ShouldCreateWithNullValue()
        {
            var accessor = new ObjectAccessor<string>();
            Assert.Null(accessor.Value);
        }

        [Fact]
        public void Constructor_WithValue_ShouldSetValue()
        {
            var accessor = new ObjectAccessor<string>("test");
            Assert.Equal("test", accessor.Value);
        }

        [Fact]
        public void Value_SetAndGet_ShouldWork()
        {
            var accessor = new ObjectAccessor<int>();
            Assert.Equal(0, accessor.Value);

            accessor.Value = 42;
            Assert.Equal(42, accessor.Value);
        }

        [Fact]
        public void Value_WithReferenceType_ShouldAllowNull()
        {
            var accessor = new ObjectAccessor<object>();
            accessor.Value = new object();
            Assert.NotNull(accessor.Value);

            accessor.Value = null;
            Assert.Null(accessor.Value);
        }

        [Fact]
        public void Value_WithValueType_ShouldHaveDefaultValue()
        {
            var accessor = new ObjectAccessor<int>();
            Assert.Equal(0, accessor.Value);
        }

        [Fact]
        public void Interface_Implementation_ShouldWork()
        {
            IObjectAccessor<string> accessor = new ObjectAccessor<string>("test");
            Assert.Equal("test", accessor.Value);

            var concreteAccessor = (ObjectAccessor<string>)accessor;
            concreteAccessor.Value = "modified";
            Assert.Equal("modified", accessor.Value);
        }
    }
}