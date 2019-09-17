﻿using System;

namespace DotCommon.Utility
{
    /// <summary>进制转换
    /// </summary>
    public static class HexUtil
    {

        /// <summary>指定进制之间转换
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="fromBase">源进制</param>
        /// <param name="toBase">目标进制</param>
        /// <returns></returns>
        public static string HexConvert(string value, int fromBase, int toBase)
        {
            int hex10 = Convert.ToInt32(value, fromBase);
            return Convert.ToString(hex10, toBase).ToUpper();
        }

        /// <summary>将其他进制转换成10进制
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="fromBase">源进制</param>
        /// <returns></returns>
        public static int ToHex10(string value, int fromBase)
        {
            return Convert.ToInt32(value, fromBase);
        }

        /// <summary>将十进制转换成其他进制
        /// </summary>
        /// <param name="value">十进制的值</param>
        /// <param name="toBase">目标进制</param>
        /// <returns></returns>
        public static string ToTargetHex(int value, int toBase)
        {
            return Convert.ToString(value, toBase).ToUpper();
        }
    }
}
