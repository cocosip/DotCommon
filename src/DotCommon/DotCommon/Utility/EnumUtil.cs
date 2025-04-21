using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace DotCommon.Utility
{
    /// <summary>
    /// 枚举工具类
    /// </summary>
    public static class EnumUtil
    {
        /// <summary>
        /// 将枚举转换成String类型
        /// </summary>
        /// <param name="e">枚举</param>
        /// <returns></returns>
        public static string GetName(Enum e)
        {
            return Enum.GetName(e.GetType(), e);
        }

        /// <summary>
        /// 将String类型转换成枚举类型
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">String值</param>
        /// <returns></returns>
        public static T ParseToEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// 获取枚举中的全部类型的数量
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <returns></returns>
        public static int GetEnumLength<T>()
        {
            return Enum.GetNames(typeof(T)).Length;
        }

        /// <summary>
        /// 获取全部的枚举
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <returns></returns>
        public static Array GetValues<T>()
        {
            return Enum.GetValues(typeof(T));
        }

        /// <summary>
        /// 判断该值是否为指定枚举中的值
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static bool IsDefined<T>(object value)
        {
            return Enum.IsDefined(typeof(T), value);
        }

        /// <summary>
        /// 获取枚举类型的描述集合
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="useInt32Key">使用Int32类型的Key值</param>
        /// <returns></returns>
        public static SortedList<string, string> GetEnumDescriptions<T>(bool useInt32Key = true)
        {
            var descriptions = new SortedList<string, string>();
            Array array = Enum.GetValues(typeof(T));
            foreach (var item in array)
            {
                var key = useInt32Key ? Convert.ToInt32(item).ToString() : item.ToString();
                var value = item.ToString();
                FieldInfo field = item.GetType().GetField(item.ToString());
                object[] attrArray = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                //如果有Desceiption属性,返回Descriotion的值
                if (attrArray != null && attrArray.Length > 0)
                {
                    DescriptionAttribute descriptionAttr = (DescriptionAttribute)attrArray[0];
                    value = descriptionAttr.Description;
                }
                descriptions.Add(key, value);
            }
            return descriptions;
        }

        /// <summary>
        /// 获取枚举的某个描述值
        /// </summary>
        /// <param name="e">枚举</param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum e)
        {
            string value = e.ToString();
            FieldInfo field = e.GetType().GetField(value);
            object[] attrArray = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            //获取描述属性
            if (attrArray == null || attrArray.Length == 0)
            {
                return value;
            }
            DescriptionAttribute descriptionAttr = (DescriptionAttribute)attrArray[0];
            return descriptionAttr.Description;
        }
    }
}
