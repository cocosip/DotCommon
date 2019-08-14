using System;
using System.Collections.Generic;
using System.Linq;

namespace DotCommon.Utility
{
    /// <summary>计算工具类
    /// </summary>
    public static class MathUtil
    {

        #region 获取集合中差值最小的两个值

        /// <summary>获取集合中差值最小的两个
        /// </summary>
        public static (T, T) GetMinMinus<T>(List<T> source, Func<T, decimal> selector)
        {
            if (source.Count < 2)
            {
                return default;
            }
            if (source.Count == 2)
            {
                return (source[0], source[1]);
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
            return (source[k], source[k + 1]);
        }

        /// <summary>获取集合中差值最小的两个
        /// </summary>
        public static (T, T) GetMinMinus<T>(List<T> source, Func<T, double> selector)
        {
            if (source.Count < 2)
            {
                return default;
            }
            if (source.Count == 2)
            {
                return (source[0], source[1]);
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
            return (source[k], source[k + 1]);
        }

        /// <summary>获取集合中差值最小的两个
        /// </summary>
        public static (T, T) GetMinMinus<T>(List<T> source, Func<T, int> selector)
        {
            if (source.Count < 2)
            {
                return default;
            }
            if (source.Count == 2)
            {
                return (source[0], source[1]);
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
            return (source[k], source[k + 1]);
        }

        /// <summary>获取集合中差值最小的两个
        /// </summary>
        public static (T, T) GetMinMinus<T>(List<T> source, Func<T, float> selector)
        {
            if (source.Count < 2)
            {
                return default;
            }
            if (source.Count == 2)
            {
                return (source[0], source[1]);
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
            return (source[k], source[k + 1]);
        }

        /// <summary>获取集合中差值最小的两个
        /// </summary>
        public static (T, T) GetMinMinus<T>(List<T> source, Func<T, long> selector)
        {
            if (source.Count < 2)
            {
                return default;
            }
            if (source.Count == 2)
            {
                return (source[0], source[1]);
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
            return (source[k], source[k + 1]);
        }


        #endregion

        /// <summary>笛卡尔积算法
        /// </summary>
        public static List<List<T>> Descartes<T>(params List<T>[] array)
        {
            int total = 1;
            foreach (var item in array)
            {
                total *= item.Count;
            }
            var result = new List<T>[total];
            int itemLoopNum = 1;
            int loopPerItem = 1;
            int now = 1;
            foreach (var arrayItem in array)
            {
                now *= arrayItem.Count;

                int index = 0;
                int currentSize = arrayItem.Count;
                itemLoopNum = total / now;
                loopPerItem = total / (itemLoopNum * currentSize);
                int myIndex = 0;

                foreach (var item in arrayItem)
                {
                    for (int i = 0; i < loopPerItem; i++)
                    {
                        if (myIndex == arrayItem.Count)
                        {
                            myIndex = 0;
                        }
                        for (int j = 0; j < itemLoopNum; j++)
                        {
                            if (result[index] == null)
                            {
                                result[index] = new List<T>();
                            }
                            result[index].Add(arrayItem[myIndex]);
                            index++;
                        }
                        myIndex++;
                    }

                }
            }

            return result.ToList();
        }
    }
}
