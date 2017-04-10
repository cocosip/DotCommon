using System;

namespace DotCommon.Utility
{
    /// <summary> 说明:强制转换
    /// </summary>
    public class ConvertFactory
    {

        /// <summary> 转换成Int32类型
        /// </summary>
        public static int ToInt32(object obj, int defaultValue)
        {
            if (obj != null)
            {
                int.TryParse(obj.ToString(), out defaultValue);
            }
            return defaultValue;
        }

        /// <summary> 转换成Int64类型
        /// </summary>
        public static long ToInt64(object obj, long defaultValue)
        {
	        if (obj != null)
	        {
		        long.TryParse(obj.ToString(), out defaultValue);
	        }
	        return defaultValue;
        }

        /// <summary>转换成Double类型
        /// </summary>
        public static double ToDouble(object obj, double defaultValue)
        {
            if (obj != null)
            {
                double.TryParse(obj.ToString(), out defaultValue);
            }
            return defaultValue;
        }
        /// <summary> 转换成double类型,并保留有效的位数
        /// </summary>
        public static double ToDouble(object obj, double defaultValue, int digit)
        {
            if (obj != null)
            {
                double.TryParse(obj.ToString(), out defaultValue);
                defaultValue = Math.Round(defaultValue, digit);
            }
            return defaultValue;
        }

        /// <summary>转换成Datetime
        /// </summary>
        public static DateTime ToDateTime(object obj, DateTime defaultValue)
        {
            var d1 = defaultValue ;
            if (obj != null)
            {
               var r= DateTime.TryParse(obj.ToString(), out d1);
               if (!r)
               {
                   return defaultValue;
               }
            }
            return d1;
        }

        /// <summary>转换成Bool类型
        /// </summary>
        public static bool ToBool(object obj, bool defaultValue)
        {
            if (obj != null)
            {
                bool.TryParse(obj.ToString(), out defaultValue);
            }
            return defaultValue;
        } 

        /// <summary>将string类型字符串转换成对应的guid
        /// </summary>
        public static Guid ToGuid(string str)
        {
            return new Guid(str);
        }

    }
}
