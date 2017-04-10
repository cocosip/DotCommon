using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace DotCommon.Reflecting
{
    /// <summary>A dynamic delegate factory.
    /// </summary>
    public class DelegateFactory
    {

        /// <summary>Creates a delegate from the given methodInfo and parameterTypes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodInfo"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        public static T CreateDelegate<T>(MethodInfo methodInfo, Type[] parameterTypes) where T : class
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }
            if (parameterTypes == null)
            {
                throw new ArgumentNullException(nameof(parameterTypes));
            }
            var parameters = methodInfo.GetParameters();
            var dynamicMethod = new DynamicMethod(
                methodInfo.Name,
                MethodAttributes.Static | MethodAttributes.Public,
                CallingConventions.Standard,
                methodInfo.ReturnType,
                parameterTypes,
                typeof(object),
                true)
            {
                InitLocals = false
            };
            var dynamicEmit = new DynamicEmit(dynamicMethod);
            if (!methodInfo.IsStatic)
            {
                dynamicEmit.LoadArgument(0);
                dynamicEmit.CastTo(typeof(object), methodInfo.DeclaringType);
            }
            for (int index = 0; index < parameters.Length; index++)
            {
                dynamicEmit.LoadArgument(index + 1);
                dynamicEmit.CastTo(parameterTypes[index + 1], parameters[index].ParameterType);
            }
            dynamicEmit.Call(methodInfo);
            dynamicEmit.Return();

            return dynamicMethod.CreateDelegate(typeof(T)) as T;
        }

        class DynamicEmit
        {
            private readonly ILGenerator _ilGenerator;
            private static readonly Dictionary<Type, OpCode> Converts = new Dictionary<Type, OpCode>();

            static DynamicEmit()
            {
                Converts.Add(typeof(sbyte), OpCodes.Conv_I1);
                Converts.Add(typeof(short), OpCodes.Conv_I2);
                Converts.Add(typeof(int), OpCodes.Conv_I4);
                Converts.Add(typeof(long), OpCodes.Conv_I8);
                Converts.Add(typeof(byte), OpCodes.Conv_U1);
                Converts.Add(typeof(ushort), OpCodes.Conv_U2);
                Converts.Add(typeof(uint), OpCodes.Conv_U4);
                Converts.Add(typeof(ulong), OpCodes.Conv_U8);
                Converts.Add(typeof(float), OpCodes.Conv_R4);
                Converts.Add(typeof(double), OpCodes.Conv_R8);
                Converts.Add(typeof(bool), OpCodes.Conv_I1);
                Converts.Add(typeof(char), OpCodes.Conv_U2);
            }
            public DynamicEmit(DynamicMethod dynamicMethod)
            {
                _ilGenerator = dynamicMethod.GetILGenerator();
            }

            public void LoadArgument(int argumentIndex)
            {
                switch (argumentIndex)
                {
                    case 0:
                        _ilGenerator.Emit(OpCodes.Ldarg_0);
                        break;
                    case 1:
                        _ilGenerator.Emit(OpCodes.Ldarg_1);
                        break;
                    case 2:
                        _ilGenerator.Emit(OpCodes.Ldarg_2);
                        break;
                    case 3:
                        _ilGenerator.Emit(OpCodes.Ldarg_3);
                        break;
                    default:
                        if (argumentIndex < 0x100)
                        {
                            _ilGenerator.Emit(OpCodes.Ldarg_S, (byte)argumentIndex);
                        }
                        else
                        {
                            _ilGenerator.Emit(OpCodes.Ldarg, argumentIndex);
                        }
                        break;
                }
            }
            public void CastTo(Type fromType, Type toType)
            {
                if (fromType != toType)
                {
                    if (toType == typeof(void))
                    {
                        if (fromType != typeof(void))
                        {
                            Pop();
                        }
                    }
                    else
                    {
                        if (fromType.GetTypeInfo().IsValueType)
                        {
                            if (toType.GetTypeInfo().IsValueType)
                            {
                                Convert(toType);
                                return;
                            }
                            _ilGenerator.Emit(OpCodes.Box, fromType);
                        }
                        CastTo(toType);
                    }
                }
            }

            private void CastTo(Type toType)
            {
                _ilGenerator.Emit(toType.GetTypeInfo().IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, toType);
            }

            private void Pop()
            {
                _ilGenerator.Emit(OpCodes.Pop);
            }

            private void Convert(Type toType)
            {
                _ilGenerator.Emit(Converts[toType]);
            }
            public void Return()
            {
                _ilGenerator.Emit(OpCodes.Ret);
            }

            public void Call(MethodInfo method)
            {
                if (method.IsFinal || !method.IsVirtual)
                {
                    _ilGenerator.EmitCall(OpCodes.Call, method, null);
                }
                else
                {
                    _ilGenerator.EmitCall(OpCodes.Callvirt, method, null);
                }
            }
        }
    }
}
