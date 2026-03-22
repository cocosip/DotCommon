using System;
using Xunit;

namespace DotCommon.Test
{
    public class DotCommonExceptionTest
    {
        [Fact]
        public void DefaultConstructor_ShouldCreateException()
        {
            var exception = new DotCommonException();
            Assert.NotNull(exception);
        }

        [Fact]
        public void WithMessage_ShouldSetMessage()
        {
            var exception = new DotCommonException("Test error message");
            Assert.Equal("Test error message", exception.Message);
        }

        [Fact]
        public void WithMessageAndInnerException_ShouldSetBoth()
        {
            var innerException = new InvalidOperationException("Inner error");
            var exception = new DotCommonException("Test error", innerException);

            Assert.Equal("Test error", exception.Message);
            Assert.Same(innerException, exception.InnerException);
        }

        [Fact]
        public void WithNullMessage_ShouldAcceptNull()
        {
            var exception = new DotCommonException(null);
            Assert.NotNull(exception);
        }

        [Fact]
        public void WithNullInnerException_ShouldAcceptNull()
        {
            var exception = new DotCommonException("Test", null);
            Assert.Equal("Test", exception.Message);
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void ShouldBeThrowable()
        {
            void ThrowException()
            {
                throw new DotCommonException("Test exception");
            }
            Assert.Throws<DotCommonException>(ThrowException);
        }

        [Fact]
        public void ShouldInheritFromException()
        {
            var exception = new DotCommonException();
            Assert.IsAssignableFrom<Exception>(exception);
        }
    }
}