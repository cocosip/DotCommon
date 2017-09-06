using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Security;

namespace DotCommon.Reflecting
{
    public class DictionaryEmit
	{
		private static readonly IDictionary<Type, Delegate> ObjectToDictionaryDict =
			new ConcurrentDictionary<Type, Delegate>();

		private static readonly IDictionary<Type, Delegate> DictionaryToObjectDict =
			new ConcurrentDictionary<Type, Delegate>();

	    private static readonly MethodInfo DictionaryContainsKey =
	        typeof (Dictionary<string, string>).GetTypeInfo().GetMethod("ContainsKey", new[] {typeof (string)});

		private static readonly MethodInfo DictionaryGetItem = typeof (Dictionary<string, string>).GetTypeInfo().GetMethod("get_Item",
			new[] {typeof (string)});


		private static readonly MethodInfo StringNull = typeof (string).GetTypeInfo().GetMethod("IsNullOrEmpty", new[] {typeof (string)});

	    private static readonly MethodInfo DictionaryAddItem = typeof (Dictionary<string, string>).GetTypeInfo().GetMethod("Add",
	        new[] {typeof (string), typeof (string)});

		private static readonly MethodInfo CultureInfo = typeof(CultureInfo).GetTypeInfo().GetMethod("get_InvariantCulture");

		private static readonly MethodInfo DatetimeToString = typeof (DateTime).GetTypeInfo().GetMethod("ToString",
			new[] {typeof (IFormatProvider)});

		#region 字典转换成对象
        /// <summary>获取字典转换成对象的委托
        /// </summary>
		public static Func<Dictionary<string, string>, T> GetObjectFunc<T>() where T : class, new()
		{
            if (typeof(ICollection).GetTypeInfo().IsAssignableFrom(typeof(T)))
            {
                throw new NotSupportedException("Not support type: ICollection");
            }
            var type = typeof(T);
			var properties = PropertyInfoUtil.GetProperties(type);
			DynamicMethod dynamicMethod = new DynamicMethod("DictionaryToObject", type,
				new Type[] { typeof(Dictionary<string, string>) }, type, true)
			{
				InitLocals = false
			};
			ILGenerator il = dynamicMethod.GetILGenerator();
			//结束标签
			Label endLabel = il.DefineLabel();
			//定义变量 var user=new User();他为第0个变量
			LocalBuilder obj = il.DeclareLocal(type);
			LocalBuilder value = il.DeclareLocal(typeof(string)); 
		    // ReSharper disable once AssignNullToNotNullAttribute
			il.Emit(OpCodes.Newobj, type.GetTypeInfo().GetConstructor(Type.EmptyTypes));
			il.Emit(OpCodes.Stloc, obj);
		    foreach (var property in properties)
		    {
		        Label endIfLabel = il.DefineLabel();
                //判断是否包含key
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldstr, property.Name);
                il.Emit(OpCodes.Callvirt, DictionaryContainsKey);
                var tmp0= il.DeclareLocal(typeof(int));
                il.Emit(OpCodes.Stloc, tmp0);
                il.Emit(OpCodes.Ldloc, tmp0);
                il.Emit(OpCodes.Brfalse_S, endIfLabel);

                //是否为空判断
                il.Emit(OpCodes.Ldarg_0);
		        il.Emit(OpCodes.Ldstr, property.Name);
		        il.Emit(OpCodes.Callvirt, DictionaryGetItem);
		        il.Emit(OpCodes.Stloc, value);
		        il.Emit(OpCodes.Ldloc, value);
		        il.Emit(OpCodes.Call, StringNull);
		        il.Emit(OpCodes.Ldc_I4_0);
		        il.Emit(OpCodes.Ceq);
		        var tmp = il.DeclareLocal(typeof (int));
		        il.Emit(OpCodes.Stloc, tmp);
		        il.Emit(OpCodes.Ldloc, tmp);
		        il.Emit(OpCodes.Brfalse_S, endIfLabel);

		        il.Emit(OpCodes.Ldloc, obj);
		        il.Emit(OpCodes.Ldloc, value);
		        Parse(il, property.PropertyType);
		        il.Emit(OpCodes.Callvirt, property.GetSetMethod());

		        il.MarkLabel(endIfLabel);
		    }
		    var result = il.DeclareLocal(type);
			il.Emit(OpCodes.Ldloc, obj);
			il.Emit(OpCodes.Stloc_S, result);
			il.Emit(OpCodes.Br_S, endLabel);
			il.MarkLabel(endLabel);
			il.Emit(OpCodes.Ldloc_S, result);
			il.Emit(OpCodes.Ret);

			var func =
				dynamicMethod.CreateDelegate(typeof(Func<Dictionary<string, string>, T>)) as Func<Dictionary<string, string>, T>;
			if (func == null)
			{
				throw new VerificationException();
			}
			return func;
		}
		private static void Parse(ILGenerator il, Type type)
		{
			if (type.GetTypeInfo().IsValueType)
			{
				//可空类型
				if (EmitUtil.IsNullable(type))
				{
					var realType = EmitUtil.GetNullableArg0(type);
					il.Emit(OpCodes.Call, realType.GetTypeInfo().GetMethod("Parse", new[] { typeof(string) }));
					// ReSharper disable once AssignNullToNotNullAttribute
					il.Emit(OpCodes.Newobj, type.GetTypeInfo().GetConstructor(new[] { realType }));
				}
				else
				{
					il.Emit(OpCodes.Call, type.GetTypeInfo().GetMethod("Parse", new[] { typeof(string) }));
				}
			}
		}

	    /// <summary>将Dictionary(string,string)字典转换为指定的不带有嵌套类型的实体
	    /// </summary>
	    public static T DictionaryToObject<T>(Dictionary<string, string> dict) where T : class, new()
	    {
	        Delegate method;
	        if (!DictionaryToObjectDict.TryGetValue(typeof(T), out method))
	        {
	            method = GetObjectFunc<T>();
                DictionaryToObjectDict.Add(typeof(T), method);
	        }
	        var func = method as Func<Dictionary<string, string>, T>;
	        return func?.Invoke(dict);
	    }

	    #endregion

        #region 将对象转换成字典类型
        /// <summary>获取将对象转换成字典类型的委托
        /// </summary>
        public static Func<T, Dictionary<string, string>> GetDictionaryFunc<T>()
		{
		    if (typeof (ICollection).GetTypeInfo().IsAssignableFrom(typeof (T)))
		    {
		        throw new NotSupportedException("Not support type: ICollection");
		    }
		    var type = typeof(Dictionary<string, string>);
			var properties = PropertyInfoUtil.GetProperties(typeof(T));
			DynamicMethod dynamicMethod = new DynamicMethod("ObjectToDictionary", type,
				new Type[] { typeof(T) }, typeof(object), true)
			{
				InitLocals = true
			};
			ILGenerator il = dynamicMethod.GetILGenerator();
			//结束标签
			Label endLabel = il.DefineLabel();
			//定义变量 var user=new User();他为第0个变量
			LocalBuilder obj = il.DeclareLocal(type);
			//定义了可空类型的变量集合
			Dictionary<Type, LocalBuilder> valueDict = new Dictionary<Type, LocalBuilder>();
			// ReSharper disable once AssignNullToNotNullAttribute
			il.Emit(OpCodes.Newobj, type.GetTypeInfo().GetConstructor(Type.EmptyTypes));
			il.Emit(OpCodes.Stloc, obj);
			int index = 0;
			foreach (var property in properties)
			{
				Label endIfLabel = il.DefineLabel();

				//string,可空类型需要null判断
				if (property.PropertyType == typeof(string))
				{
					il.Emit(OpCodes.Ldarg_0);
					il.Emit(OpCodes.Callvirt, property.GetGetMethod());
					il.Emit(OpCodes.Ldnull);
					il.Emit(OpCodes.Cgt_Un);
					var tmp = il.DeclareLocal(typeof(bool));
					il.Emit(OpCodes.Stloc, tmp);
					il.Emit(OpCodes.Ldloc, tmp);
					il.Emit(OpCodes.Brfalse_S, endIfLabel);
				}
				//可空类型
				if (EmitUtil.IsNullable(property.PropertyType))
				{
					var hasValue = il.DeclareLocal(typeof(bool));
					il.Emit(OpCodes.Ldarg_0);
					il.Emit(OpCodes.Callvirt, property.GetGetMethod());
					var local = GetLocal(il, property.PropertyType, valueDict);
					il.Emit(OpCodes.Stloc_S, local);
					il.Emit(OpCodes.Ldloca_S, local);
					il.Emit(OpCodes.Call, property.PropertyType.GetTypeInfo().GetMethod("get_HasValue", new Type[] { }));
					if (index == 0)
					{
						il.Emit(OpCodes.Stloc, hasValue);
						il.Emit(OpCodes.Ldloc, hasValue);
					}
					else
					{
						il.Emit(OpCodes.Stloc_S, hasValue);
						il.Emit(OpCodes.Ldloc_S, hasValue);
					}
					index++;
					il.Emit(OpCodes.Brfalse_S, endIfLabel);
				}

				il.Emit(OpCodes.Ldloc, obj);
				il.Emit(OpCodes.Ldstr, property.Name);
				il.Emit(OpCodes.Ldarg_0);
				il.Emit(OpCodes.Callvirt, property.GetGetMethod()); //取对象属性值
				PropertyToString(il, property.PropertyType, valueDict);
				il.Emit(OpCodes.Callvirt, DictionaryAddItem);
				il.MarkLabel(endIfLabel);
			}
			var result = il.DeclareLocal(type);
			il.Emit(OpCodes.Ldloc, obj);
			il.Emit(OpCodes.Stloc_S, result);
			il.Emit(OpCodes.Br_S, endLabel);
			il.MarkLabel(endLabel);
			il.Emit(OpCodes.Ldloc_S, result);
			il.Emit(OpCodes.Ret);

			var func =
				dynamicMethod.CreateDelegate(typeof(Func<T, Dictionary<string, string>>)) as Func<T, Dictionary<string, string>>;
			if (func == null)
			{
				throw new VerificationException();
			}
			return func;
		}

	    /// <summary>将对象转换成Dictionary(string,string)类型
	    /// </summary>
	    public static Dictionary<string, string> ObjectToDictionary<T>(T t)
	    {
	        Delegate method;
	        if (!ObjectToDictionaryDict.TryGetValue(typeof(T), out method))
	        {
	            method = GetDictionaryFunc<T>();
                ObjectToDictionaryDict.Add(typeof(T), method);
	        }
	        var func = method as Func<T, Dictionary<string, string>>;
	        return func?.Invoke(t);
	    }

	    private static LocalBuilder GetLocal(ILGenerator il, Type type, Dictionary<Type, LocalBuilder> valueDict)
		{
			LocalBuilder localBuilder;
			//可空类型或者Datetime类型,都需要将参数存储起来
			if (valueDict.ContainsKey(type))
			{
				localBuilder = valueDict[type];
			}
			else
			{
				localBuilder = il.DeclareLocal(type);
				valueDict.Add(type, localBuilder);
			}
			return localBuilder;
		}
		private static LocalBuilder GetRealLocal(ILGenerator il, Type type, Dictionary<Type, LocalBuilder> realValueDict)
		{
			LocalBuilder localBuilder;
			Type realType = type;
			//可空类型或者Datetime类型,都需要将参数存储起来
			if (EmitUtil.IsNullable(type))
			{
				realType = EmitUtil.GetNullableArg0(type);
			}
			if (realValueDict.ContainsKey(realType))
			{
				localBuilder = realValueDict[realType];
			}
			else
			{
				localBuilder = il.DeclareLocal(realType);
				realValueDict.Add(realType, localBuilder);
			}
			return localBuilder;
		}
		private static void PropertyToString(ILGenerator il, Type type, Dictionary<Type, LocalBuilder> valueDict)
		{
			//值类型和可空类型
			if (type.GetTypeInfo().IsValueType)
			{
				//不可空类型,直接创建新的,可空类型就先从字典中取,取不到再新建

				var localBuilder = GetLocal(il, type, valueDict);
				il.Emit(OpCodes.Stloc_S, localBuilder);
				il.Emit(OpCodes.Ldloca_S, localBuilder);

				if (type == typeof(DateTime) || type == typeof(DateTime?))
				{
					//可空datetime类型
					if (type == typeof(DateTime?))
					{
						il.Emit(OpCodes.Call, type.GetTypeInfo().GetMethod("get_Value", new Type[] { }));
						var realLocal = GetRealLocal(il, type, valueDict);
						il.Emit(OpCodes.Stloc_S, realLocal);
						il.Emit(OpCodes.Ldloca_S, realLocal);
					}
					il.Emit(OpCodes.Call, CultureInfo);
					il.Emit(OpCodes.Call, DatetimeToString);
				}
				else
				{
					if (EmitUtil.IsNullable(type))
					{
						il.Emit(OpCodes.Call, type.GetTypeInfo().GetMethod("get_Value", new Type[] { }));
						var realLocal = GetRealLocal(il, type, valueDict);
						il.Emit(OpCodes.Stloc_S, realLocal);
						il.Emit(OpCodes.Ldloca_S, realLocal);
					}

					il.Emit(OpCodes.Call, EmitUtil.GetNullableArg0(type).GetTypeInfo().GetMethod("ToString", new Type[] { }));
				}
			}
		}
        #endregion
	}
}
