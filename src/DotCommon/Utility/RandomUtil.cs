using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Utility
{
    /// <summary>随机数相关操作
    /// </summary>
    public class RandomUtil
    {

        private static readonly char[] RandomArray =
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c',
            'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y',
            'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
            'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };

        private static readonly Dictionary<RandomStringType, Tuple<int, int>> RandomRanges = new Dictionary
            <RandomStringType, Tuple<int, int>>()
        {
            [RandomStringType.Number] = new Tuple<int, int>(0, 9),
            [RandomStringType.LowerLetter] = new Tuple<int, int>(10, 35),
            [RandomStringType.UpperLetter] = new Tuple<int, int>(36, 61),
            [RandomStringType.Fix] = new Tuple<int, int>(0, 61),
            [RandomStringType.NumberLower] = new Tuple<int, int>(0, 35),
            [RandomStringType.NumberUpper] = new Tuple<int, int>(36, 71),
            [RandomStringType.Letter] = new Tuple<int, int>(10, 61)
        };


        #region 生成随机数的种子

        /// <summary>生成随机数的种子
        /// </summary>
        public static int GetRandomSeed(int len = 8)
        {
            var bytes = new byte[len];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(bytes);
            }
            return BitConverter.ToInt32(bytes, 0);
        }

        #endregion

        #region 生成一个指定范围的随机整数

        /// <summary>生成一个指定范围的随机整数
        /// </summary>
        public static int GetRandomInt(int minNum, int maxNum)
        {
            var rd = new Random();
            return rd.Next(minNum, maxNum);
        }

        #endregion

        #region 对一个数组进行随机排序

        /// <summary> 对一个数组进行随机排序
        /// </summary>am>
        public static void GetRandomArray<T>(T[] arr)
        {
            //对数组进行随机排序的算法:随机选择两个位置，将两个位置上的值交换
            //交换的次数,这里使用数组的长度作为交换次数
            var changeCount = arr.Length/2;
            var rd = new Random(GetRandomSeed());
            //开始交换
            for (int i = 0; i < changeCount; i++)
            {
                //生成两个随机数位置
                var randomNum1 = rd.Next(0, arr.Length);
                var randomNum2 = rd.Next(0, arr.Length);
                //交换两个随机数位置的值
                var temp = arr[randomNum1];
                arr[randomNum1] = arr[randomNum2];
                arr[randomNum2] = temp;
            }
        }

        #endregion

        /// <summary>生成随机字符串
        /// </summary>
        public static string GetRandomStr(int length, RandomStringType randomStringType = RandomStringType.Number)
        {
            var rd = new Random(GetRandomSeed());
            var sb=new StringBuilder();
            var range = RandomRanges[randomStringType];
            for (var i = 0; i < length; i++)
            {
                //生成随机的当前索引
                var index = rd.Next(range.Item1, range.Item2);
                sb.Append(RandomArray[index]);
            }
            return sb.ToString();
        }


    }

    public enum RandomStringType
    {
        Number = 1,
        LowerLetter = 2,
        UpperLetter = 4,
        Fix = 8,
        NumberLower = 16,
        NumberUpper = 32,
        Letter = 64
    }
}
