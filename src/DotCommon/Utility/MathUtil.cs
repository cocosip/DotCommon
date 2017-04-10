using System;
using System.Collections.Generic;
using System.Linq;

namespace DotCommon.Utility
{
    /// <summary>计算工具类
    /// </summary>
    public class MathUtil
    {

        #region 获取集合中差值最小的两个值

        /// <summary>获取集合中差值最小的两个
        /// </summary>
        public static Tuple<T, T> GetMinMinus<T>(List<T> source, Func<T, decimal> selector)
        {
            if (source.Count < 2)
            {
                return null;
            }
            if (source.Count == 2)
            {
                return new Tuple<T, T>(source[0], source[1]);
            }
            source = source.OrderBy(selector).ToList();
            var difference = Math.Abs(selector(source[0]) - selector(source[1]));
            int k = 0; //数组的下标
            for (int i = 1; i < source.Count - 1; i++)
            {
                var currentDifference = Math.Abs(selector(source[i]) - selector(source[i + 1]));
                if (difference > currentDifference)
                {
                    difference = currentDifference;
                    k = i;
                }
            }
            return new Tuple<T, T>(source[k], source[k + 1]);
        }

        /// <summary>获取集合中差值最小的两个
        /// </summary>
        public static Tuple<T, T> GetMinMinus<T>(List<T> source, Func<T, double> selector)
        {
            if (source.Count < 2)
            {
                return null;
            }
            if (source.Count == 2)
            {
                return new Tuple<T, T>(source[0], source[1]);
            }
            source = source.OrderBy(selector).ToList();
            var difference = Math.Abs(selector(source[0]) - selector(source[1]));
            int k = 0; //数组的下标
            for (int i = 1; i < source.Count - 1; i++)
            {
                var currentDifference = Math.Abs(selector(source[i]) - selector(source[i + 1]));
                if (difference > currentDifference)
                {
                    difference = currentDifference;
                    k = i;
                }
            }
            return new Tuple<T, T>(source[k], source[k + 1]);
        }

        /// <summary>获取集合中差值最小的两个
        /// </summary>
        public static Tuple<T, T> GetMinMinus<T>(List<T> source, Func<T, int> selector)
        {
            if (source.Count < 2)
            {
                return null;
            }
            if (source.Count == 2)
            {
                return new Tuple<T, T>(source[0], source[1]);
            }
            source = source.OrderBy(selector).ToList();
            var difference = Math.Abs(selector(source[0]) - selector(source[1]));
            int k = 0; //数组的下标
            for (int i = 1; i < source.Count - 1; i++)
            {
                var currentDifference = Math.Abs(selector(source[i]) - selector(source[i + 1]));
                if (difference > currentDifference)
                {
                    difference = currentDifference;
                    k = i;
                }
            }
            return new Tuple<T, T>(source[k], source[k + 1]);
        }

        /// <summary>获取集合中差值最小的两个
        /// </summary>
        public static Tuple<T, T> GetMinMinus<T>(List<T> source, Func<T, float> selector)
        {
            if (source.Count < 2)
            {
                return null;
            }
            if (source.Count == 2)
            {
                return new Tuple<T, T>(source[0], source[1]);
            }
            source = source.OrderBy(selector).ToList();
            var difference = Math.Abs(selector(source[0]) - selector(source[1]));
            int k = 0; //数组的下标
            for (int i = 1; i < source.Count - 1; i++)
            {
                var currentDifference = Math.Abs(selector(source[i]) - selector(source[i + 1]));
                if (difference > currentDifference)
                {
                    difference = currentDifference;
                    k = i;
                }
            }
            return new Tuple<T, T>(source[k], source[k + 1]);
        }

        /// <summary>获取集合中差值最小的两个
        /// </summary>
        public static Tuple<T, T> GetMinMinus<T>(List<T> source, Func<T, long> selector)
        {
            if (source.Count < 2)
            {
                return null;
            }
            if (source.Count == 2)
            {
                return new Tuple<T, T>(source[0], source[1]);
            }
            source = source.OrderBy(selector).ToList();
            var difference = Math.Abs(selector(source[0]) - selector(source[1]));
            int k = 0; //数组的下标
            for (int i = 1; i < source.Count - 1; i++)
            {
                var currentDifference = Math.Abs(selector(source[i]) - selector(source[i + 1]));
                if (difference > currentDifference)
                {
                    difference = currentDifference;
                    k = i;
                }
            }
            return new Tuple<T, T>(source[k], source[k + 1]);
        }

        /// <summary>获取集合中差值最小的两个
        /// </summary>
        public static Tuple<T, T> GetMinMinus<T>(List<T> source, Func<T, byte> selector)
        {
            if (source.Count < 2)
            {
                return null;
            }
            if (source.Count == 2)
            {
                return new Tuple<T, T>(source[0], source[1]);
            }
            source = source.OrderBy(selector).ToList();
            var difference = Math.Abs(selector(source[0]) - selector(source[1]));
            int k = 0; //数组的下标
            for (int i = 1; i < source.Count - 1; i++)
            {
                var currentDifference = Math.Abs(selector(source[i]) - selector(source[i + 1]));
                if (difference > currentDifference)
                {
                    difference = currentDifference;
                    k = i;
                }
            }
            return new Tuple<T, T>(source[k], source[k + 1]);
        }

        #endregion

    }
}
