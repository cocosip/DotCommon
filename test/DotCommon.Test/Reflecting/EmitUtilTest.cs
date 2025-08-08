using System;
using System.Reflection;
using System.Reflection.Emit;
using DotCommon.Reflecting;
using Xunit;

namespace DotCommon.Test.Reflecting
{
    /// <summary>
    /// Unit tests for EmitUtil class
    /// </summary>
    public class EmitUtilTest
    {
        #region Nullable Type Operations Tests

        [Fact]
        public void IsNullable_WithNullableInt_ShouldReturnTrue()
        {
            // Arrange
            var nullableIntType = typeof(int?);

            // Act
            var result = EmitUtil.IsNullable(nullableIntType);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsNullable_WithRegularInt_ShouldReturnFalse()
        {
            // Arrange
            var intType = typeof(int);

            // Act
            var result = EmitUtil.IsNullable(intType);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsNullable_WithReferenceType_ShouldReturnFalse()
        {
            // Arrange
            var stringType = typeof(string);

            // Act
            var result = EmitUtil.IsNullable(stringType);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsNullable_WithNullType_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => EmitUtil.IsNullable(null));
        }

        [Fact]
        public void GetNullableUnderlyingType_WithNullableInt_ShouldReturnInt()
        {
            // Arrange
            var nullableIntType = typeof(int?);

            // Act
            var result = EmitUtil.GetNullableUnderlyingType(nullableIntType);

            // Assert
            Assert.Equal(typeof(int), result);
        }

        [Fact]
        public void GetNullableUnderlyingType_WithRegularInt_ShouldReturnInt()
        {
            // Arrange
            var intType = typeof(int);

            // Act
            var result = EmitUtil.GetNullableUnderlyingType(intType);

            // Assert
            Assert.Equal(typeof(int), result);
        }

        [Fact]
        public void GetNullableUnderlyingType_WithNullableDateTime_ShouldReturnDateTime()
        {
            // Arrange
            var nullableDateTimeType = typeof(DateTime?);

            // Act
            var result = EmitUtil.GetNullableUnderlyingType(nullableDateTimeType);

            // Assert
            Assert.Equal(typeof(DateTime), result);
        }

        [Fact]
        public void GetNullableUnderlyingType_WithNullType_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => EmitUtil.GetNullableUnderlyingType(null));
        }

        #endregion

        #region OpCode Utilities Tests

        [Fact]
        public void GetLoadOpCode_WithInt_ShouldReturnLdindI4()
        {
            // Act
            var result = EmitUtil.GetLoadOpCode(typeof(int));

            // Assert
            Assert.Equal(OpCodes.Ldind_I4, result);
        }

        [Fact]
        public void GetLoadOpCode_WithLong_ShouldReturnLdindI8()
        {
            // Act
            var result = EmitUtil.GetLoadOpCode(typeof(long));

            // Assert
            Assert.Equal(OpCodes.Ldind_I8, result);
        }

        [Fact]
        public void GetLoadOpCode_WithFloat_ShouldReturnLdindR4()
        {
            // Act
            var result = EmitUtil.GetLoadOpCode(typeof(float));

            // Assert
            Assert.Equal(OpCodes.Ldind_R4, result);
        }

        [Fact]
        public void GetLoadOpCode_WithDouble_ShouldReturnLdindR8()
        {
            // Act
            var result = EmitUtil.GetLoadOpCode(typeof(double));

            // Assert
            Assert.Equal(OpCodes.Ldind_R8, result);
        }

        [Fact]
        public void GetLoadOpCode_WithString_ShouldReturnLdindRef()
        {
            // Act
            var result = EmitUtil.GetLoadOpCode(typeof(string));

            // Assert
            Assert.Equal(OpCodes.Ldind_Ref, result);
        }

        [Fact]
        public void GetLoadOpCode_WithNullType_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => EmitUtil.GetLoadOpCode(null));
        }

        [Fact]
        public void GetStoreOpCode_WithInt_ShouldReturnStindI4()
        {
            // Act
            var result = EmitUtil.GetStoreOpCode(typeof(int));

            // Assert
            Assert.Equal(OpCodes.Stind_I4, result);
        }

        [Fact]
        public void GetStoreOpCode_WithLong_ShouldReturnStindI8()
        {
            // Act
            var result = EmitUtil.GetStoreOpCode(typeof(long));

            // Assert
            Assert.Equal(OpCodes.Stind_I8, result);
        }

        [Fact]
        public void GetStoreOpCode_WithFloat_ShouldReturnStindR4()
        {
            // Act
            var result = EmitUtil.GetStoreOpCode(typeof(float));

            // Assert
            Assert.Equal(OpCodes.Stind_R4, result);
        }

        [Fact]
        public void GetStoreOpCode_WithDouble_ShouldReturnStindR8()
        {
            // Act
            var result = EmitUtil.GetStoreOpCode(typeof(double));

            // Assert
            Assert.Equal(OpCodes.Stind_R8, result);
        }

        [Fact]
        public void GetStoreOpCode_WithString_ShouldReturnStindRef()
        {
            // Act
            var result = EmitUtil.GetStoreOpCode(typeof(string));

            // Assert
            Assert.Equal(OpCodes.Stind_Ref, result);
        }

        [Fact]
        public void GetStoreOpCode_WithNullType_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => EmitUtil.GetStoreOpCode(null));
        }

        [Fact]
        public void GetLoadConstantOpCode_WithMinusOne_ShouldReturnLdcI4M1()
        {
            // Act
            var result = EmitUtil.GetLoadConstantOpCode(-1);

            // Assert
            Assert.Equal(OpCodes.Ldc_I4_M1, result);
        }

        [Fact]
        public void GetLoadConstantOpCode_WithZero_ShouldReturnLdcI40()
        {
            // Act
            var result = EmitUtil.GetLoadConstantOpCode(0);

            // Assert
            Assert.Equal(OpCodes.Ldc_I4_0, result);
        }

        [Fact]
        public void GetLoadConstantOpCode_WithOne_ShouldReturnLdcI41()
        {
            // Act
            var result = EmitUtil.GetLoadConstantOpCode(1);

            // Assert
            Assert.Equal(OpCodes.Ldc_I4_1, result);
        }

        [Fact]
        public void GetLoadConstantOpCode_WithEight_ShouldReturnLdcI48()
        {
            // Act
            var result = EmitUtil.GetLoadConstantOpCode(8);

            // Assert
            Assert.Equal(OpCodes.Ldc_I4_8, result);
        }

        [Fact]
        public void GetLoadConstantOpCode_WithSmallValue_ShouldReturnLdcI4S()
        {
            // Act
            var result = EmitUtil.GetLoadConstantOpCode(100);

            // Assert
            Assert.Equal(OpCodes.Ldc_I4_S, result);
        }

        [Fact]
        public void GetLoadConstantOpCode_WithLargeValue_ShouldReturnLdcI4()
        {
            // Act
            var result = EmitUtil.GetLoadConstantOpCode(1000);

            // Assert
            Assert.Equal(OpCodes.Ldc_I4, result);
        }

        [Fact]
        public void GetLoadArgumentOpCode_WithZero_ShouldReturnLdarg0()
        {
            // Act
            var result = EmitUtil.GetLoadArgumentOpCode(0);

            // Assert
            Assert.Equal(OpCodes.Ldarg_0, result);
        }

        [Fact]
        public void GetLoadArgumentOpCode_WithThree_ShouldReturnLdarg3()
        {
            // Act
            var result = EmitUtil.GetLoadArgumentOpCode(3);

            // Assert
            Assert.Equal(OpCodes.Ldarg_3, result);
        }

        [Fact]
        public void GetLoadArgumentOpCode_WithSmallIndex_ShouldReturnLdargS()
        {
            // Act
            var result = EmitUtil.GetLoadArgumentOpCode(4);

            // Assert
            Assert.Equal(OpCodes.Ldarg_S, result);
        }

        [Fact]
        public void GetLoadArgumentOpCode_WithLargeIndex_ShouldReturnLdarg()
        {
            // Act
            var result = EmitUtil.GetLoadArgumentOpCode(256);

            // Assert
            Assert.Equal(OpCodes.Ldarg, result);
        }

        [Fact]
        public void GetStoreArgumentOpCode_WithSmallIndex_ShouldReturnStargS()
        {
            // Act
            var result = EmitUtil.GetStoreArgumentOpCode(0);

            // Assert
            Assert.Equal(OpCodes.Starg_S, result);
        }

        [Fact]
        public void GetStoreArgumentOpCode_WithLargeIndex_ShouldReturnStarg()
        {
            // Act
            var result = EmitUtil.GetStoreArgumentOpCode(256);

            // Assert
            Assert.Equal(OpCodes.Starg, result);
        }

        [Fact]
        public void GetLoadLocalOpCode_WithZero_ShouldReturnLdloc0()
        {
            // Act
            var result = EmitUtil.GetLoadLocalOpCode(0);

            // Assert
            Assert.Equal(OpCodes.Ldloc_0, result);
        }

        [Fact]
        public void GetLoadLocalOpCode_WithThree_ShouldReturnLdloc3()
        {
            // Act
            var result = EmitUtil.GetLoadLocalOpCode(3);

            // Assert
            Assert.Equal(OpCodes.Ldloc_3, result);
        }

        [Fact]
        public void GetLoadLocalOpCode_WithSmallIndex_ShouldReturnLdlocS()
        {
            // Act
            var result = EmitUtil.GetLoadLocalOpCode(4);

            // Assert
            Assert.Equal(OpCodes.Ldloc_S, result);
        }

        [Fact]
        public void GetLoadLocalOpCode_WithLargeIndex_ShouldReturnLdloc()
        {
            // Act
            var result = EmitUtil.GetLoadLocalOpCode(256);

            // Assert
            Assert.Equal(OpCodes.Ldloc, result);
        }

        [Fact]
        public void GetStoreLocalOpCode_WithZero_ShouldReturnStloc0()
        {
            // Act
            var result = EmitUtil.GetStoreLocalOpCode(0);

            // Assert
            Assert.Equal(OpCodes.Stloc_0, result);
        }

        [Fact]
        public void GetStoreLocalOpCode_WithThree_ShouldReturnStloc3()
        {
            // Act
            var result = EmitUtil.GetStoreLocalOpCode(3);

            // Assert
            Assert.Equal(OpCodes.Stloc_3, result);
        }

        [Fact]
        public void GetStoreLocalOpCode_WithSmallIndex_ShouldReturnStlocS()
        {
            // Act
            var result = EmitUtil.GetStoreLocalOpCode(4);

            // Assert
            Assert.Equal(OpCodes.Stloc_S, result);
        }

        [Fact]
        public void GetStoreLocalOpCode_WithLargeIndex_ShouldReturnStloc()
        {
            // Act
            var result = EmitUtil.GetStoreLocalOpCode(256);

            // Assert
            Assert.Equal(OpCodes.Stloc, result);
        }

        #endregion

        #region Type Size and Alignment Tests

        [Theory]
        [InlineData(typeof(bool), 1)]
        [InlineData(typeof(byte), 1)]
        [InlineData(typeof(sbyte), 1)]
        [InlineData(typeof(short), 2)]
        [InlineData(typeof(ushort), 2)]
        [InlineData(typeof(char), 2)]
        [InlineData(typeof(int), 4)]
        [InlineData(typeof(uint), 4)]
        [InlineData(typeof(float), 4)]
        [InlineData(typeof(long), 8)]
        [InlineData(typeof(ulong), 8)]
        [InlineData(typeof(double), 8)]
        [InlineData(typeof(decimal), 16)]
        public void GetTypeSize_WithPrimitiveTypes_ShouldReturnCorrectSize(Type type, int expectedSize)
        {
            // Act
            var result = EmitUtil.GetTypeSize(type);

            // Assert
            Assert.Equal(expectedSize, result);
        }

        [Fact]
        public void GetTypeSize_WithReferenceType_ShouldReturnPointerSize()
        {
            // Act
            var result = EmitUtil.GetTypeSize(typeof(string));

            // Assert
            Assert.Equal(IntPtr.Size, result);
        }

        [Fact]
        public void GetTypeSize_WithNullType_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => EmitUtil.GetTypeSize(null));
        }

        [Theory]
        [InlineData(typeof(int), false)]
        [InlineData(typeof(string), false)]
        [InlineData(typeof(int?), true)]
        public void RequiresSpecialHandling_WithVariousTypes_ShouldReturnCorrectResult(Type type, bool expectedResult)
        {
            // Act
            var result = EmitUtil.RequiresSpecialHandling(type);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void RequiresSpecialHandling_WithNullType_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => EmitUtil.RequiresSpecialHandling(null));
        }

        #endregion

        #region IL Generation Helpers Tests

        [Fact]
        public void EmitLoadDefaultValue_WithValueType_ShouldEmitCorrectInstructions()
        {
            // Arrange
            var dynamicMethod = new DynamicMethod("TestMethod", typeof(int), Type.EmptyTypes);
            var il = dynamicMethod.GetILGenerator();

            // Act
            EmitUtil.EmitLoadDefaultValue(il, typeof(int));
            il.Emit(OpCodes.Ret);

            // Assert
            var func = (Func<int>)dynamicMethod.CreateDelegate(typeof(Func<int>));
            var result = func();
            Assert.Equal(0, result);
        }

        [Fact]
        public void EmitLoadDefaultValue_WithReferenceType_ShouldEmitCorrectInstructions()
        {
            // Arrange
            var dynamicMethod = new DynamicMethod("TestMethod", typeof(string), Type.EmptyTypes);
            var il = dynamicMethod.GetILGenerator();

            // Act
            EmitUtil.EmitLoadDefaultValue(il, typeof(string));
            il.Emit(OpCodes.Ret);

            // Assert
            var func = (Func<string>)dynamicMethod.CreateDelegate(typeof(Func<string>));
            var result = func();
            Assert.Null(result);
        }

        [Fact]
        public void EmitLoadDefaultValue_WithNullIL_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => EmitUtil.EmitLoadDefaultValue(null, typeof(int)));
        }

        [Fact]
        public void EmitLoadDefaultValue_WithNullType_ShouldThrowArgumentNullException()
        {
            // Arrange
            var dynamicMethod = new DynamicMethod("TestMethod", typeof(int), Type.EmptyTypes);
            var il = dynamicMethod.GetILGenerator();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => EmitUtil.EmitLoadDefaultValue(il, null));
        }

        [Theory]
        [InlineData(42)]
        [InlineData(-1)]
        [InlineData(0)]
        public void EmitLoadConstant_WithInt_ShouldEmitCorrectInstructions(int value)
        {
            // Arrange
            var dynamicMethod = new DynamicMethod("TestMethod", typeof(int), Type.EmptyTypes);
            var il = dynamicMethod.GetILGenerator();

            // Act
            EmitUtil.EmitLoadConstant(il, value);
            il.Emit(OpCodes.Ret);

            // Assert
            var func = (Func<int>)dynamicMethod.CreateDelegate(typeof(Func<int>));
            var result = func();
            Assert.Equal(value, result);
        }

        [Fact]
        public void EmitLoadConstant_WithString_ShouldEmitCorrectInstructions()
        {
            // Arrange
            var dynamicMethod = new DynamicMethod("TestMethod", typeof(string), Type.EmptyTypes);
            var il = dynamicMethod.GetILGenerator();
            var testString = "Hello World";

            // Act
            EmitUtil.EmitLoadConstant(il, testString);
            il.Emit(OpCodes.Ret);

            // Assert
            var func = (Func<string>)dynamicMethod.CreateDelegate(typeof(Func<string>));
            var result = func();
            Assert.Equal(testString, result);
        }

        [Fact]
        public void EmitLoadConstant_WithNull_ShouldEmitCorrectInstructions()
        {
            // Arrange
            var dynamicMethod = new DynamicMethod("TestMethod", typeof(object), Type.EmptyTypes);
            var il = dynamicMethod.GetILGenerator();

            // Act
            EmitUtil.EmitLoadConstant(il, null);
            il.Emit(OpCodes.Ret);

            // Assert
            var func = (Func<object>)dynamicMethod.CreateDelegate(typeof(Func<object>));
            var result = func();
            Assert.Null(result);
        }

        [Fact]
        public void EmitLoadConstant_WithNullIL_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => EmitUtil.EmitLoadConstant(null, 42));
        }

        [Fact]
        public void EmitBoxIfNeeded_WithValueType_ShouldEmitBoxInstruction()
        {
            // Arrange
            var dynamicMethod = new DynamicMethod("TestMethod", typeof(object), Type.EmptyTypes);
            var il = dynamicMethod.GetILGenerator();

            // Act
            il.Emit(OpCodes.Ldc_I4, 42);
            EmitUtil.EmitBoxIfNeeded(il, typeof(int));
            il.Emit(OpCodes.Ret);

            // Assert
            var func = (Func<object>)dynamicMethod.CreateDelegate(typeof(Func<object>));
            var result = func();
            Assert.Equal(42, result);
            Assert.IsType<int>(result);
        }

        [Fact]
        public void EmitBoxIfNeeded_WithReferenceType_ShouldNotEmitBoxInstruction()
        {
            // Arrange
            var dynamicMethod = new DynamicMethod("TestMethod", typeof(object), Type.EmptyTypes);
            var il = dynamicMethod.GetILGenerator();

            // Act
            il.Emit(OpCodes.Ldstr, "test");
            EmitUtil.EmitBoxIfNeeded(il, typeof(string));
            il.Emit(OpCodes.Ret);

            // Assert
            var func = (Func<object>)dynamicMethod.CreateDelegate(typeof(Func<object>));
            var result = func();
            Assert.Equal("test", result);
        }

        [Fact]
        public void EmitBoxIfNeeded_WithNullIL_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => EmitUtil.EmitBoxIfNeeded(null, typeof(int)));
        }

        [Fact]
        public void EmitBoxIfNeeded_WithNullType_ShouldThrowArgumentNullException()
        {
            // Arrange
            var dynamicMethod = new DynamicMethod("TestMethod", typeof(object), Type.EmptyTypes);
            var il = dynamicMethod.GetILGenerator();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => EmitUtil.EmitBoxIfNeeded(il, null));
        }

        [Fact]
        public void EmitUnboxIfNeeded_WithValueType_ShouldEmitUnboxInstruction()
        {
            // Arrange
            var dynamicMethod = new DynamicMethod("TestMethod", typeof(int), new[] { typeof(object) });
            var il = dynamicMethod.GetILGenerator();

            // Act
            il.Emit(OpCodes.Ldarg_0);
            EmitUtil.EmitUnboxIfNeeded(il, typeof(int));
            il.Emit(OpCodes.Ret);

            // Assert
            var func = (Func<object, int>)dynamicMethod.CreateDelegate(typeof(Func<object, int>));
            var result = func((object)42);
            Assert.Equal(42, result);
        }

        [Fact]
        public void EmitUnboxIfNeeded_WithReferenceType_ShouldEmitCastInstruction()
        {
            // Arrange
            var dynamicMethod = new DynamicMethod("TestMethod", typeof(string), new[] { typeof(object) });
            var il = dynamicMethod.GetILGenerator();

            // Act
            il.Emit(OpCodes.Ldarg_0);
            EmitUtil.EmitUnboxIfNeeded(il, typeof(string));
            il.Emit(OpCodes.Ret);

            // Assert
            var func = (Func<object, string>)dynamicMethod.CreateDelegate(typeof(Func<object, string>));
            var result = func("test");
            Assert.Equal("test", result);
        }

        [Fact]
        public void EmitUnboxIfNeeded_WithNullIL_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => EmitUtil.EmitUnboxIfNeeded(null, typeof(int)));
        }

        [Fact]
        public void EmitUnboxIfNeeded_WithNullType_ShouldThrowArgumentNullException()
        {
            // Arrange
            var dynamicMethod = new DynamicMethod("TestMethod", typeof(int), new[] { typeof(object) });
            var il = dynamicMethod.GetILGenerator();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => EmitUtil.EmitUnboxIfNeeded(il, null));
        }

        [Fact]
        public void EmitConversion_WithSameTypes_ShouldNotEmitConversion()
        {
            // Arrange
            var dynamicMethod = new DynamicMethod("TestMethod", typeof(int), Type.EmptyTypes);
            var il = dynamicMethod.GetILGenerator();

            // Act
            il.Emit(OpCodes.Ldc_I4, 42);
            EmitUtil.EmitConversion(il, typeof(int), typeof(int));
            il.Emit(OpCodes.Ret);

            // Assert
            var func = (Func<int>)dynamicMethod.CreateDelegate(typeof(Func<int>));
            var result = func();
            Assert.Equal(42, result);
        }

        [Fact]
        public void EmitConversion_WithIntToByte_ShouldEmitConversion()
        {
            // Arrange
            var dynamicMethod = new DynamicMethod("TestMethod", typeof(byte), Type.EmptyTypes);
            var il = dynamicMethod.GetILGenerator();

            // Act
            il.Emit(OpCodes.Ldc_I4, 42);
            EmitUtil.EmitConversion(il, typeof(int), typeof(byte));
            il.Emit(OpCodes.Ret);

            // Assert
            var func = (Func<byte>)dynamicMethod.CreateDelegate(typeof(Func<byte>));
            var result = func();
            Assert.Equal((byte)42, result);
        }

        [Fact]
        public void EmitConversion_WithNullIL_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => EmitUtil.EmitConversion(null, typeof(int), typeof(byte)));
        }

        [Fact]
        public void EmitConversion_WithNullFromType_ShouldThrowArgumentNullException()
        {
            // Arrange
            var dynamicMethod = new DynamicMethod("TestMethod", typeof(byte), Type.EmptyTypes);
            var il = dynamicMethod.GetILGenerator();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => EmitUtil.EmitConversion(il, null, typeof(byte)));
        }

        [Fact]
        public void EmitConversion_WithNullToType_ShouldThrowArgumentNullException()
        {
            // Arrange
            var dynamicMethod = new DynamicMethod("TestMethod", typeof(byte), Type.EmptyTypes);
            var il = dynamicMethod.GetILGenerator();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => EmitUtil.EmitConversion(il, typeof(int), null));
        }

        #endregion

        #region Null Parameter Tests

        [Fact]
        public void AllMethods_WithNullParameters_ShouldThrowArgumentNullException()
        {
            // Test all methods that should throw ArgumentNullException with null parameters
            var dynamicMethod = new DynamicMethod("TestMethod", typeof(void), Type.EmptyTypes);
            var il = dynamicMethod.GetILGenerator();

            // IsNullable
            Assert.Throws<ArgumentNullException>(() => EmitUtil.IsNullable(null));

            // GetNullableUnderlyingType
            Assert.Throws<ArgumentNullException>(() => EmitUtil.GetNullableUnderlyingType(null));

            // GetLoadOpCode
            Assert.Throws<ArgumentNullException>(() => EmitUtil.GetLoadOpCode(null));

            // GetStoreOpCode
            Assert.Throws<ArgumentNullException>(() => EmitUtil.GetStoreOpCode(null));

            // GetTypeSize
            Assert.Throws<ArgumentNullException>(() => EmitUtil.GetTypeSize(null));

            // RequiresSpecialHandling
            Assert.Throws<ArgumentNullException>(() => EmitUtil.RequiresSpecialHandling(null));

            // EmitLoadDefaultValue
            Assert.Throws<ArgumentNullException>(() => EmitUtil.EmitLoadDefaultValue(null, typeof(int)));
            Assert.Throws<ArgumentNullException>(() => EmitUtil.EmitLoadDefaultValue(il, null));

            // EmitLoadConstant
            Assert.Throws<ArgumentNullException>(() => EmitUtil.EmitLoadConstant(null, 42));

            // EmitBoxIfNeeded
            Assert.Throws<ArgumentNullException>(() => EmitUtil.EmitBoxIfNeeded(null, typeof(int)));
            Assert.Throws<ArgumentNullException>(() => EmitUtil.EmitBoxIfNeeded(il, null));

            // EmitUnboxIfNeeded
            Assert.Throws<ArgumentNullException>(() => EmitUtil.EmitUnboxIfNeeded(null, typeof(int)));
            Assert.Throws<ArgumentNullException>(() => EmitUtil.EmitUnboxIfNeeded(il, null));

            // EmitConversion
            Assert.Throws<ArgumentNullException>(() => EmitUtil.EmitConversion(null, typeof(int), typeof(byte)));
            Assert.Throws<ArgumentNullException>(() => EmitUtil.EmitConversion(il, null, typeof(byte)));
            Assert.Throws<ArgumentNullException>(() => EmitUtil.EmitConversion(il, typeof(int), null));
        }

        #endregion
    }
}