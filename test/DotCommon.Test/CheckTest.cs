using System;
using System.Collections.Generic;
using Xunit;

namespace DotCommon.Test
{
    public class CheckTest
    {
        [Fact]
        public void NotNull_WithNullValue_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Check.NotNull<object>(null, "param"));
        }

        [Fact]
        public void NotNull_WithValue_ShouldReturnValue()
        {
            var value = new object();
            var result = Check.NotNull(value, "param");
            Assert.Same(value, result);
        }

        [Fact]
        public void NotNull_WithMessage_ShouldThrowWithMessage()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Check.NotNull<object>(null, "param", "custom message"));
            Assert.Contains("custom message", ex.Message);
        }

        [Theory]
        [InlineData(null, "param", true)]
        [InlineData("test", "param", false)]
        public void NotNull_String_ShouldValidateCorrectly(string value, string paramName, bool shouldThrow)
        {
            if (shouldThrow)
            {
                Assert.Throws<ArgumentNullException>(() => Check.NotNull(value, paramName));
            }
            else
            {
                var result = Check.NotNull(value, paramName);
                Assert.Equal(value, result);
            }
        }

        [Theory]
        [InlineData("abc", 5, 0, false)]
        [InlineData("abc", 2, 0, true)]
        [InlineData("abc", 5, 4, true)]
        public void NotNull_WithLength_ShouldValidateLength(string value, int maxLength, int minLength, bool shouldThrow)
        {
            if (shouldThrow)
            {
                Assert.Throws<ArgumentException>(() => Check.NotNull(value, "param", maxLength, minLength));
            }
            else
            {
                var result = Check.NotNull(value, "param", maxLength, minLength);
                Assert.Equal(value, result);
            }
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData("", true)]
        [InlineData("  ", true)]
        [InlineData("test", false)]
        public void NotNullOrWhiteSpace_ShouldValidateCorrectly(string value, bool shouldThrow)
        {
            if (shouldThrow)
            {
                Assert.Throws<ArgumentException>(() => Check.NotNullOrWhiteSpace(value, "param"));
            }
            else
            {
                var result = Check.NotNullOrWhiteSpace(value, "param");
                Assert.Equal(value, result);
            }
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData("", true)]
        [InlineData("test", false)]
        public void NotNullOrEmpty_String_ShouldValidateCorrectly(string value, bool shouldThrow)
        {
            if (shouldThrow)
            {
                Assert.Throws<ArgumentException>(() => Check.NotNullOrEmpty(value, "param"));
            }
            else
            {
                var result = Check.NotNullOrEmpty(value, "param");
                Assert.Equal(value, result);
            }
        }

        [Fact]
        public void NotNullOrEmpty_Collection_WithNull_ShouldThrow()
        {
            Assert.Throws<ArgumentException>(() => Check.NotNullOrEmpty<int>(null, "param"));
        }

        [Fact]
        public void NotNullOrEmpty_Collection_WithEmpty_ShouldThrow()
        {
            Assert.Throws<ArgumentException>(() => Check.NotNullOrEmpty(new List<int>(), "param"));
        }

        [Fact]
        public void NotNullOrEmpty_Collection_WithItems_ShouldReturn()
        {
            var list = new List<int> { 1, 2, 3 };
            var result = Check.NotNullOrEmpty(list, "param");
            Assert.Same(list, result);
        }

        [Fact]
        public void AssignableTo_WithValidType_ShouldReturn()
        {
            var result = Check.AssignableTo<IComparable>(typeof(int), "param");
            Assert.Equal(typeof(int), result);
        }

        [Fact]
        public void AssignableTo_WithInvalidType_ShouldThrow()
        {
            Assert.Throws<ArgumentException>(() => Check.AssignableTo<IComparable>(typeof(object), "param"));
        }

        [Theory]
        [InlineData((short)5, false)]
        [InlineData((short)0, true)]
        [InlineData((short)-1, true)]
        public void Positive_Short_ShouldValidate(short value, bool shouldThrow)
        {
            if (shouldThrow)
            {
                Assert.Throws<ArgumentException>(() => Check.Positive(value, "param"));
            }
            else
            {
                var result = Check.Positive(value, "param");
                Assert.Equal(value, result);
            }
        }

        [Theory]
        [InlineData(5, false)]
        [InlineData(0, true)]
        [InlineData(-1, true)]
        public void Positive_Int_ShouldValidate(int value, bool shouldThrow)
        {
            if (shouldThrow)
            {
                Assert.Throws<ArgumentException>(() => Check.Positive(value, "param"));
            }
            else
            {
                var result = Check.Positive(value, "param");
                Assert.Equal(value, result);
            }
        }

        [Theory]
        [InlineData(5L, false)]
        [InlineData(0L, true)]
        [InlineData(-1L, true)]
        public void Positive_Long_ShouldValidate(long value, bool shouldThrow)
        {
            if (shouldThrow)
            {
                Assert.Throws<ArgumentException>(() => Check.Positive(value, "param"));
            }
            else
            {
                var result = Check.Positive(value, "param");
                Assert.Equal(value, result);
            }
        }

        [Theory]
        [InlineData(5.0f, false)]
        [InlineData(0.0f, true)]
        [InlineData(-1.0f, true)]
        public void Positive_Float_ShouldValidate(float value, bool shouldThrow)
        {
            if (shouldThrow)
            {
                Assert.Throws<ArgumentException>(() => Check.Positive(value, "param"));
            }
            else
            {
                var result = Check.Positive(value, "param");
                Assert.Equal(value, result);
            }
        }

        [Theory]
        [InlineData(5.0, false)]
        [InlineData(0.0, true)]
        [InlineData(-1.0, true)]
        public void Positive_Double_ShouldValidate(double value, bool shouldThrow)
        {
            if (shouldThrow)
            {
                Assert.Throws<ArgumentException>(() => Check.Positive(value, "param"));
            }
            else
            {
                var result = Check.Positive(value, "param");
                Assert.Equal(value, result);
            }
        }

        [Theory]
        [InlineData(5.0, false)]
        [InlineData(0.0, true)]
        [InlineData(-1.0, true)]
        public void Positive_Decimal_ShouldValidate(decimal value, bool shouldThrow)
        {
            if (shouldThrow)
            {
                Assert.Throws<ArgumentException>(() => Check.Positive(value, "param"));
            }
            else
            {
                var result = Check.Positive(value, "param");
                Assert.Equal(value, result);
            }
        }

        [Theory]
        [InlineData(5, 1, 10, false)]
        [InlineData(0, 1, 10, true)]
        [InlineData(15, 1, 10, true)]
        public void Range_Int_ShouldValidate(int value, int min, int max, bool shouldThrow)
        {
            if (shouldThrow)
            {
                Assert.Throws<ArgumentException>(() => Check.Range(value, "param", min, max));
            }
            else
            {
                var result = Check.Range(value, "param", min, max);
                Assert.Equal(value, result);
            }
        }

        [Theory]
        [InlineData(5L, 1L, 10L, false)]
        [InlineData(0L, 1L, 10L, true)]
        public void Range_Long_ShouldValidate(long value, long min, long max, bool shouldThrow)
        {
            if (shouldThrow)
            {
                Assert.Throws<ArgumentException>(() => Check.Range(value, "param", min, max));
            }
            else
            {
                var result = Check.Range(value, "param", min, max);
                Assert.Equal(value, result);
            }
        }

        [Theory]
        [InlineData(5.0, 1.0, 10.0, false)]
        [InlineData(0.0, 1.0, 10.0, true)]
        public void Range_Double_ShouldValidate(double value, double min, double max, bool shouldThrow)
        {
            if (shouldThrow)
            {
                Assert.Throws<ArgumentException>(() => Check.Range(value, "param", min, max));
            }
            else
            {
                var result = Check.Range(value, "param", min, max);
                Assert.Equal(value, result);
            }
        }

        [Fact]
        public void NotDefaultOrNull_WithNull_ShouldThrow()
        {
            int? value = null;
            Assert.Throws<ArgumentException>(() => Check.NotDefaultOrNull(value, "param"));
        }

        [Fact]
        public void NotDefaultOrNull_WithDefault_ShouldThrow()
        {
            int? value = 0;
            Assert.Throws<ArgumentException>(() => Check.NotDefaultOrNull(value, "param"));
        }

        [Fact]
        public void NotDefaultOrNull_WithValue_ShouldReturn()
        {
            int? value = 5;
            var result = Check.NotDefaultOrNull(value, "param");
            Assert.Equal(5, result);
        }

        [Theory]
        [InlineData(null, 5, 0, false)]
        [InlineData("test", 5, 0, false)]
        [InlineData("te", 5, 3, true)]
        [InlineData("test", 2, 0, true)]
        public void Length_ShouldValidate(string value, int maxLength, int minLength, bool shouldThrow)
        {
            if (shouldThrow)
            {
                Assert.Throws<ArgumentException>(() => Check.Length(value, "param", maxLength, minLength));
            }
            else
            {
                var result = Check.Length(value, "param", maxLength, minLength);
                Assert.Equal(value, result);
            }
        }

        [Theory]
        [InlineData("test", 5, 0, false)]
        [InlineData("test", 2, 0, true)]
        [InlineData("te", 5, 3, true)]
        public void NotNullOrWhiteSpace_WithLength_ShouldValidate(string value, int maxLength, int minLength, bool shouldThrow)
        {
            if (shouldThrow)
            {
                Assert.Throws<ArgumentException>(() => Check.NotNullOrWhiteSpace(value, "param", maxLength, minLength));
            }
            else
            {
                var result = Check.NotNullOrWhiteSpace(value, "param", maxLength, minLength);
                Assert.Equal(value, result);
            }
        }

        [Theory]
        [InlineData("test", 5, 0, false)]
        [InlineData("test", 2, 0, true)]
        [InlineData("te", 5, 3, true)]
        public void NotNullOrEmpty_WithLength_ShouldValidate(string value, int maxLength, int minLength, bool shouldThrow)
        {
            if (shouldThrow)
            {
                Assert.Throws<ArgumentException>(() => Check.NotNullOrEmpty(value, "param", maxLength, minLength));
            }
            else
            {
                var result = Check.NotNullOrEmpty(value, "param", maxLength, minLength);
                Assert.Equal(value, result);
            }
        }

        [Theory]
        [InlineData((short)5, (short)1, (short)10, false)]
        [InlineData((short)0, (short)1, (short)10, true)]
        [InlineData((short)15, (short)1, (short)10, true)]
        public void Range_Short_ShouldValidate(short value, short min, short max, bool shouldThrow)
        {
            if (shouldThrow)
            {
                Assert.Throws<ArgumentException>(() => Check.Range(value, "param", min, max));
            }
            else
            {
                var result = Check.Range(value, "param", min, max);
                Assert.Equal(value, result);
            }
        }

        [Theory]
        [InlineData(5.0f, 1.0f, 10.0f, false)]
        [InlineData(0.0f, 1.0f, 10.0f, true)]
        [InlineData(15.0f, 1.0f, 10.0f, true)]
        public void Range_Float_ShouldValidate(float value, float min, float max, bool shouldThrow)
        {
            if (shouldThrow)
            {
                Assert.Throws<ArgumentException>(() => Check.Range(value, "param", min, max));
            }
            else
            {
                var result = Check.Range(value, "param", min, max);
                Assert.Equal(value, result);
            }
        }

        [Theory]
        [InlineData(5.0, 1.0, 10.0, false)]
        [InlineData(0.0, 1.0, 10.0, true)]
        [InlineData(15.0, 1.0, 10.0, true)]
        public void Range_Decimal_ShouldValidate(decimal value, decimal min, decimal max, bool shouldThrow)
        {
            if (shouldThrow)
            {
                Assert.Throws<ArgumentException>(() => Check.Range(value, "param", min, max));
            }
            else
            {
                var result = Check.Range(value, "param", min, max);
                Assert.Equal(value, result);
            }
        }
    }
}