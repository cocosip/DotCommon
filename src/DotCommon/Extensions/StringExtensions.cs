using System;

namespace DotCommon.Extensions
{
    public static class StringExtensions
    {
        /// <summary>返回平台无关的Hashcode
        /// </summary>
        public static int GetStringHashcode(this string s)
        {
            if (string.IsNullOrEmpty(s)) return 0;

            unchecked
            {
                int hash = 23;
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (char c in s)
                {
                    hash = (hash << 5) - hash + c;
                }
                if (hash < 0)
                {
                    hash = Math.Abs(hash);
                }
                return hash;
            }
        }
    }
}
