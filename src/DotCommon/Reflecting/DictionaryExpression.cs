using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DotCommon.Reflecting
{
    public static class DictionaryExpression
    {
        private static readonly IDictionary<Type, Delegate> ObjectToDictionaryDict =
            new ConcurrentDictionary<Type, Delegate>();

        private static readonly IDictionary<Type, Delegate> DictionaryToObjectDict =
            new ConcurrentDictionary<Type, Delegate>();
        private static readonly IDictionary<Type, string> ConvertMethodNames = new ConcurrentDictionary<Type, string>()
        {
            [typeof(short)] = "ToInt16",
            [typeof(int)] = "ToInt32",
            [typeof(long)] = "ToInt64",
            [typeof(double)] = "ToDouble",
            [typeof(decimal)] = "ToDecimal",
            [typeof(string)] = "ToString",
            [typeof(DateTime)] = "ToDateTime",
            [typeof(byte)] = "ToByte",
            [typeof(char)] = "ToChar",
            [typeof(bool)] = "ToBoolean",

            [typeof(short?)] = "ToInt16",
            [typeof(int?)] = "ToInt32",
            [typeof(long?)] = "ToInt64",
            [typeof(double?)] = "ToDouble",
            [typeof(decimal?)] = "ToDecimal",
            [typeof(DateTime?)] = "ToDateTime",
            [typeof(byte?)] = "ToByte",
            [typeof(char?)] = "ToChar",
            [typeof(bool?)] = "ToBoolean"
        };

        private static string GetConvertName(Type type)
        {
            return ConvertMethodNames[type];
        }

        /// <summary>将字典类型转换成对象的委托
        /// </summary>
        public static Func<Dictionary<string, object>, T> GetObjectFunc<T>() where T : class, new()
        {
            var sourceType = typeof(Dictionary<string, object>);
            var targetType = typeof(T);
            // define the parameter
            var parameterExpr = Expression.Parameter(sourceType, "x");
            //collect the body
            var bodyExprs = new List<Expression>();
            //初始化对象 var t=new T();
            var objectExpr = Expression.Variable(targetType, "obj");
            var newObjectExpr = Expression.New(targetType);
            var assignObjectExpr = Expression.Assign(objectExpr, newObjectExpr);
            bodyExprs.Add(assignObjectExpr);
            //获取T的全部公有属性
            var properties = PropertyInfoUtil.GetProperties(targetType);
            foreach (var property in properties)
            {
                var nameExpr = Expression.Constant(property.Name);
                var fieldExpr = Expression.PropertyOrField(objectExpr, property.Name);
                //将object转换为对应的属性类型,int,double...
                // 要调用索引必须先获取PropertyType类型
                PropertyInfo indexer =
                (from p in parameterExpr.Type.GetTypeInfo().GetDefaultMembers().OfType<PropertyInfo>()
                     // This check is probably useless. You can't overload on return value in C#.
                 where p.PropertyType == typeof(object)
                 let q = p.GetIndexParameters()
                 // Here we can search for the exact overload. Length is the number of "parameters" of the indexer, and then we can check for their type.
                 where q.Length == 1 && q[0].ParameterType == typeof(string)
                 select p).Single();

                var indexValueExpr = Expression.Property(parameterExpr, indexer, nameExpr);


                //属性值
                var castIndexValueExpr = Expression.Convert(indexValueExpr, property.PropertyType);


                //调用Convert.ChangeType(object,xxx);
                //var convertMethodExpr = Expression.Call(null,
                //    typeof(Convert).GetTypeInfo()
                //        .GetMethod(GetConvertName(property.PropertyType), new Type[] { typeof(object) }), indexValueExpr);

                var assignFieldExpr = Expression.Assign(fieldExpr, castIndexValueExpr);
                //code: if(obj.Id!=null){ ... }
                var notNullExpr = Expression.IfThen(Expression.NotEqual(indexValueExpr, Expression.Constant(null)),
                    assignFieldExpr);

                //contains方法
                var containMethodExpr = Expression.Call(parameterExpr,
                    sourceType.GetTypeInfo().GetMethod("ContainsKey", new[] { typeof(string) }), nameExpr);
                //code: if(x.contains("xxx")){ if(x["id"]!=null){ x["id"]=obj.Id; } }
                var ifContainExpr = Expression.IfThen(containMethodExpr, notNullExpr);
                bodyExprs.Add(ifContainExpr);

            }
            //返回
            bodyExprs.Add(objectExpr);
            var methodBodyExpr = Expression.Block(targetType, new[] { objectExpr }, bodyExprs);
            // code: entity => { ... }
            var lambdaExpr = Expression.Lambda<Func<Dictionary<string, object>, T>>(methodBodyExpr, parameterExpr);
            var func = lambdaExpr.Compile();
            return func;
        }

        /// <summary>将字典类型转换成对象
        /// </summary>
        public static T DictionaryToObject<T>(Dictionary<string, object> dict) where T : class, new()
        {
            Delegate method;
            if (!DictionaryToObjectDict.TryGetValue(typeof(T), out method))
            {
                method = GetObjectFunc<T>();
                DictionaryToObjectDict.Add(typeof(T), method);
            }
            var func = method as Func<Dictionary<string, object>, T>;
            return func?.Invoke(dict);
        }

        /// <summary>将对象转换成字典类型委托
        /// </summary>
        public static Func<T, Dictionary<string, object>> GetDictionaryFunc<T>() where T : class, new()
        {
            var sourceType = typeof(T);
            var targetType = typeof(Dictionary<string, object>);
            // define the parameter
            var parameterExpr = Expression.Parameter(sourceType, "x");
            //collect the body
            var bodyExprs = new List<Expression>();
            //code:var dict=new Dictionary<string,object>();
            var dictExpr = Expression.Variable(targetType, "dict");
            var newDictExpr = Expression.New(targetType);
            var assignDictExpr = Expression.Assign(dictExpr, newDictExpr);
            bodyExprs.Add(assignDictExpr);
            var properties = PropertyInfoUtil.GetProperties(sourceType);
            foreach (var property in properties)
            {
                // code: if(x.UserId!=null){ dict.Add("UserId",(object)1); }
                var nameExpr = Expression.Constant(property.Name);
                var castValueExpr = Expression.Convert(Expression.Property(parameterExpr, property), typeof(object));
                // code: dict.Add("UserId",xxx);
                var addMethodExpr = Expression.Call(dictExpr,
                    targetType.GetTypeInfo().GetMethod("Add", new[] { typeof(string), typeof(object) }), nameExpr,
                    castValueExpr);
                //添加Null
                var addNullMethodExpr = Expression.Call(dictExpr, targetType.GetTypeInfo().GetMethod("Add", new[] { typeof(string), typeof(object) }), nameExpr, Expression.Constant(null));

                //code: if(x.UserId!=null){ ... }else{ //直接添加null};
                var ifNotNullElseExpr = Expression.IfThenElse(Expression.NotEqual(castValueExpr, Expression.Constant(null)),
                    addMethodExpr, addNullMethodExpr);
                bodyExprs.Add(ifNotNullElseExpr);
            }
            //返回
            bodyExprs.Add(dictExpr);
            var methodBodyExpr = Expression.Block(targetType, new[] { dictExpr }, bodyExprs);
            // code: entity => { ... }
            var lambdaExpr = Expression.Lambda<Func<T, Dictionary<string, object>>>(methodBodyExpr, parameterExpr);
            var func = lambdaExpr.Compile();
            return func;
        }

        /// <summary>将对象转换成字典类型
        /// </summary>
        public static Dictionary<string, object> ObjectToDictionary<T>(T obj) where T : class, new()
        {
            Delegate method;
            if (!ObjectToDictionaryDict.TryGetValue(typeof(T), out method))
            {
                method = GetDictionaryFunc<T>();
                ObjectToDictionaryDict.Add(typeof(T), method);
            }
            var func = method as Func<T, Dictionary<string, object>>;
            return func?.Invoke(obj);
        }

    }
}
