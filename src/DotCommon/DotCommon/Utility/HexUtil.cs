using System;

namespace DotCommon.Utility
{
    /// <summary>
    /// Hexadecimal conversion utility class
    /// </summary>
    public static class HexUtil
    {

        /// <summary>
        /// Converts between specified bases
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <param name="fromBase">The source base</param>
        /// <param name="toBase">The target base</param>
        /// <returns>The converted value as a string</returns>
        public static string HexConvert(string value, int fromBase, int toBase)
        {
            int hex10 = Convert.ToInt32(value, fromBase);
            return Convert.ToString(hex10, toBase).ToUpper();
        }

        /// <summary>
        /// Converts other bases to decimal (base 10)
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <param name="fromBase">The source base</param>
        /// <returns>The decimal representation of the value</returns>
        public static int ToHex10(string value, int fromBase)
        {
            return Convert.ToInt32(value, fromBase);
        }

        /// <summary>
        /// Converts decimal (base 10) to other bases
        /// </summary>
        /// <param name="value">The decimal value to convert</param>
        /// <param name="toBase">The target base</param>
        /// <returns>The converted value as a string</returns>
        public static string ToTargetHex(int value, int toBase)
        {
            return Convert.ToString(value, toBase).ToUpper();
        }
    }
}