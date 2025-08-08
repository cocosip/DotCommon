using System;
using System.Reflection;
using System.Reflection.Emit;

namespace DotCommon.Reflecting
{
    /// <summary>
    /// Emit utility class for dynamic code generation and IL operations
    /// </summary>
    public static class EmitUtil
    {
        #region Nullable Type Operations

        /// <summary>
        /// Determines whether the specified type is a nullable value type
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns>true if the type is nullable; otherwise, false</returns>
        /// <exception cref="ArgumentNullException">Thrown when type is null</exception>
        public static bool IsNullable(Type type)
        {
            Check.NotNull(type, nameof(type));
            return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Gets the underlying type of a nullable type, or returns the original type if it's not nullable
        /// </summary>
        /// <param name="type">The type to get the underlying type from</param>
        /// <returns>The underlying type if nullable; otherwise, the original type</returns>
        /// <exception cref="ArgumentNullException">Thrown when type is null</exception>
        public static Type GetNullableUnderlyingType(Type type)
        {
            Check.NotNull(type, nameof(type));
            return IsNullable(type) ? type.GenericTypeArguments[0] : type;
        }

        #endregion

        #region OpCode Utilities

        /// <summary>
        /// Gets the appropriate load instruction opcode for the specified type
        /// </summary>
        /// <param name="type">The type to get the load instruction for</param>
        /// <returns>The appropriate load instruction opcode</returns>
        /// <exception cref="ArgumentNullException">Thrown when type is null</exception>
        public static OpCode GetLoadOpCode(Type type)
        {
            Check.NotNull(type, nameof(type));
            
            if (type == typeof(int) || type == typeof(uint) || type == typeof(bool) || type == typeof(byte) || 
                type == typeof(sbyte) || type == typeof(short) || type == typeof(ushort) || type == typeof(char))
            {
                return OpCodes.Ldind_I4;
            }
            
            if (type == typeof(long) || type == typeof(ulong))
            {
                return OpCodes.Ldind_I8;
            }
            
            if (type == typeof(float))
            {
                return OpCodes.Ldind_R4;
            }
            
            if (type == typeof(double))
            {
                return OpCodes.Ldind_R8;
            }
            
            return OpCodes.Ldind_Ref;
        }

        /// <summary>
        /// Gets the appropriate store instruction opcode for the specified type
        /// </summary>
        /// <param name="type">The type to get the store instruction for</param>
        /// <returns>The appropriate store instruction opcode</returns>
        /// <exception cref="ArgumentNullException">Thrown when type is null</exception>
        public static OpCode GetStoreOpCode(Type type)
        {
            Check.NotNull(type, nameof(type));
            
            if (type == typeof(int) || type == typeof(uint) || type == typeof(bool) || type == typeof(byte) || 
                type == typeof(sbyte) || type == typeof(short) || type == typeof(ushort) || type == typeof(char))
            {
                return OpCodes.Stind_I4;
            }
            
            if (type == typeof(long) || type == typeof(ulong))
            {
                return OpCodes.Stind_I8;
            }
            
            if (type == typeof(float))
            {
                return OpCodes.Stind_R4;
            }
            
            if (type == typeof(double))
            {
                return OpCodes.Stind_R8;
            }
            
            return OpCodes.Stind_Ref;
        }

        /// <summary>
        /// Gets the appropriate load constant instruction opcode for the specified value
        /// </summary>
        /// <param name="value">The constant value</param>
        /// <returns>The appropriate load constant instruction opcode</returns>
        public static OpCode GetLoadConstantOpCode(int value)
        {
            switch (value)
            {
                case -1: return OpCodes.Ldc_I4_M1;
                case 0: return OpCodes.Ldc_I4_0;
                case 1: return OpCodes.Ldc_I4_1;
                case 2: return OpCodes.Ldc_I4_2;
                case 3: return OpCodes.Ldc_I4_3;
                case 4: return OpCodes.Ldc_I4_4;
                case 5: return OpCodes.Ldc_I4_5;
                case 6: return OpCodes.Ldc_I4_6;
                case 7: return OpCodes.Ldc_I4_7;
                case 8: return OpCodes.Ldc_I4_8;
                default:
                    if (value >= sbyte.MinValue && value <= sbyte.MaxValue)
                        return OpCodes.Ldc_I4_S;
                    return OpCodes.Ldc_I4;
            }
        }

        /// <summary>
        /// Gets the appropriate load argument instruction opcode for the specified argument index
        /// </summary>
        /// <param name="index">The argument index</param>
        /// <returns>The appropriate load argument instruction opcode</returns>
        public static OpCode GetLoadArgumentOpCode(int index)
        {
            switch (index)
            {
                case 0: return OpCodes.Ldarg_0;
                case 1: return OpCodes.Ldarg_1;
                case 2: return OpCodes.Ldarg_2;
                case 3: return OpCodes.Ldarg_3;
                default:
                    if (index <= byte.MaxValue)
                        return OpCodes.Ldarg_S;
                    return OpCodes.Ldarg;
            }
        }

        /// <summary>
        /// Gets the appropriate store argument instruction opcode for the specified argument index
        /// </summary>
        /// <param name="index">The argument index</param>
        /// <returns>The appropriate store argument instruction opcode</returns>
        public static OpCode GetStoreArgumentOpCode(int index)
        {
            if (index <= byte.MaxValue)
                return OpCodes.Starg_S;
            return OpCodes.Starg;
        }

        /// <summary>
        /// Gets the appropriate load local variable instruction opcode for the specified local index
        /// </summary>
        /// <param name="index">The local variable index</param>
        /// <returns>The appropriate load local variable instruction opcode</returns>
        public static OpCode GetLoadLocalOpCode(int index)
        {
            switch (index)
            {
                case 0: return OpCodes.Ldloc_0;
                case 1: return OpCodes.Ldloc_1;
                case 2: return OpCodes.Ldloc_2;
                case 3: return OpCodes.Ldloc_3;
                default:
                    if (index <= byte.MaxValue)
                        return OpCodes.Ldloc_S;
                    return OpCodes.Ldloc;
            }
        }

        /// <summary>
        /// Gets the appropriate store local variable instruction opcode for the specified local index
        /// </summary>
        /// <param name="index">The local variable index</param>
        /// <returns>The appropriate store local variable instruction opcode</returns>
        public static OpCode GetStoreLocalOpCode(int index)
        {
            switch (index)
            {
                case 0: return OpCodes.Stloc_0;
                case 1: return OpCodes.Stloc_1;
                case 2: return OpCodes.Stloc_2;
                case 3: return OpCodes.Stloc_3;
                default:
                    if (index <= byte.MaxValue)
                        return OpCodes.Stloc_S;
                    return OpCodes.Stloc;
            }
        }

        #endregion

        #region Type Size and Alignment for IL

        /// <summary>
        /// Gets the size in bytes of the specified type for IL operations
        /// </summary>
        /// <param name="type">The type to get the size for</param>
        /// <returns>The size in bytes</returns>
        /// <exception cref="ArgumentNullException">Thrown when type is null</exception>
        public static int GetTypeSize(Type type)
        {
            Check.NotNull(type, nameof(type));
            
            if (type == typeof(bool) || type == typeof(byte) || type == typeof(sbyte))
                return 1;
            
            if (type == typeof(short) || type == typeof(ushort) || type == typeof(char))
                return 2;
            
            if (type == typeof(int) || type == typeof(uint) || type == typeof(float))
                return 4;
            
            if (type == typeof(long) || type == typeof(ulong) || type == typeof(double))
                return 8;
            
            if (type == typeof(decimal))
                return 16;
            
            if (type.IsPointer || type.IsByRef || !type.IsValueType)
                return IntPtr.Size;
            
            // For other value types, we need to calculate the size
            return System.Runtime.InteropServices.Marshal.SizeOf(type);
        }

        /// <summary>
        /// Determines whether the specified type requires special handling in IL generation
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns>true if the type requires special handling; otherwise, false</returns>
        /// <exception cref="ArgumentNullException">Thrown when type is null</exception>
        public static bool RequiresSpecialHandling(Type type)
        {
            Check.NotNull(type, nameof(type));
            
            return type.IsByRef || type.IsPointer || IsNullable(type) || 
                   type.IsGenericParameter || type.ContainsGenericParameters;
        }

        #endregion

        #region IL Generation Helpers

        /// <summary>
        /// Emits IL instructions to load the default value for the specified type onto the evaluation stack
        /// </summary>
        /// <param name="il">The IL generator</param>
        /// <param name="type">The type to load the default value for</param>
        /// <exception cref="ArgumentNullException">Thrown when il or type is null</exception>
        public static void EmitLoadDefaultValue(ILGenerator il, Type type)
        {
            Check.NotNull(il, nameof(il));
            Check.NotNull(type, nameof(type));
            
            if (type.IsValueType)
            {
                var local = il.DeclareLocal(type);
                il.Emit(OpCodes.Ldloca_S, local);
                il.Emit(OpCodes.Initobj, type);
                il.Emit(OpCodes.Ldloc, local);
            }
            else
            {
                il.Emit(OpCodes.Ldnull);
            }
        }

        /// <summary>
        /// Emits IL instructions to load a constant value onto the evaluation stack
        /// </summary>
        /// <param name="il">The IL generator</param>
        /// <param name="value">The constant value to load</param>
        /// <exception cref="ArgumentNullException">Thrown when il is null</exception>
        public static void EmitLoadConstant(ILGenerator il, object value)
        {
            Check.NotNull(il, nameof(il));
            
            if (value == null)
            {
                il.Emit(OpCodes.Ldnull);
                return;
            }
            
            var type = value.GetType();
            
            if (type == typeof(int))
            {
                var intValue = (int)value;
                var opCode = GetLoadConstantOpCode(intValue);
                if (opCode == OpCodes.Ldc_I4_S)
                    il.Emit(opCode, (sbyte)intValue);
                else if (opCode == OpCodes.Ldc_I4)
                    il.Emit(opCode, intValue);
                else
                    il.Emit(opCode);
            }
            else if (type == typeof(long))
            {
                il.Emit(OpCodes.Ldc_I8, (long)value);
            }
            else if (type == typeof(float))
            {
                il.Emit(OpCodes.Ldc_R4, (float)value);
            }
            else if (type == typeof(double))
            {
                il.Emit(OpCodes.Ldc_R8, (double)value);
            }
            else if (type == typeof(string))
            {
                il.Emit(OpCodes.Ldstr, (string)value);
            }
            else if (type == typeof(bool))
            {
                il.Emit((bool)value ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
            }
            else
            {
                throw new ArgumentException($"Unsupported constant type: {type}", nameof(value));
            }
        }

        /// <summary>
        /// Emits IL instructions to box a value type if necessary
        /// </summary>
        /// <param name="il">The IL generator</param>
        /// <param name="type">The type to box</param>
        /// <exception cref="ArgumentNullException">Thrown when il or type is null</exception>
        public static void EmitBoxIfNeeded(ILGenerator il, Type type)
        {
            Check.NotNull(il, nameof(il));
            Check.NotNull(type, nameof(type));
            
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Box, type);
            }
        }

        /// <summary>
        /// Emits IL instructions to unbox a value type if necessary
        /// </summary>
        /// <param name="il">The IL generator</param>
        /// <param name="type">The type to unbox</param>
        /// <exception cref="ArgumentNullException">Thrown when il or type is null</exception>
        public static void EmitUnboxIfNeeded(ILGenerator il, Type type)
        {
            Check.NotNull(il, nameof(il));
            Check.NotNull(type, nameof(type));
            
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, type);
            }
            else
            {
                il.Emit(OpCodes.Castclass, type);
            }
        }

        /// <summary>
        /// Emits IL instructions to convert between numeric types
        /// </summary>
        /// <param name="il">The IL generator</param>
        /// <param name="fromType">The source type</param>
        /// <param name="toType">The target type</param>
        /// <exception cref="ArgumentNullException">Thrown when il, fromType, or toType is null</exception>
        public static void EmitConversion(ILGenerator il, Type fromType, Type toType)
        {
            Check.NotNull(il, nameof(il));
            Check.NotNull(fromType, nameof(fromType));
            Check.NotNull(toType, nameof(toType));
            
            if (fromType == toType)
                return;
            
            // Handle nullable types
            var actualFromType = GetNullableUnderlyingType(fromType);
            var actualToType = GetNullableUnderlyingType(toType);
            
            if (actualFromType == actualToType)
                return;
            
            // Emit conversion opcodes for numeric types
            if (actualToType == typeof(byte))
                il.Emit(OpCodes.Conv_U1);
            else if (actualToType == typeof(sbyte))
                il.Emit(OpCodes.Conv_I1);
            else if (actualToType == typeof(short))
                il.Emit(OpCodes.Conv_I2);
            else if (actualToType == typeof(ushort))
                il.Emit(OpCodes.Conv_U2);
            else if (actualToType == typeof(int))
                il.Emit(OpCodes.Conv_I4);
            else if (actualToType == typeof(uint))
                il.Emit(OpCodes.Conv_U4);
            else if (actualToType == typeof(long))
                il.Emit(OpCodes.Conv_I8);
            else if (actualToType == typeof(ulong))
                il.Emit(OpCodes.Conv_U8);
            else if (actualToType == typeof(float))
                il.Emit(OpCodes.Conv_R4);
            else if (actualToType == typeof(double))
                il.Emit(OpCodes.Conv_R8);
        }

        #endregion
    }
}
