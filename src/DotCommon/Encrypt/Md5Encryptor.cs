﻿using System;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Encrypt
{
    /// <summary>Md5加密
    /// </summary>
    public static class Md5Encryptor
    {
        /// <summary>Md5加密
        /// </summary>
        public static string GetMd5(byte[] data)
        {
            using (var md5 = MD5.Create())
            {
                //其他的代码写法: md5.ComputeHash(data).Aggregate("", (current, b) => current + b.ToString("X2"))
                return BitConverter.ToString(md5.ComputeHash(data)).Replace("-", "");

            }
        }

        /// <summary>Md5加密
        /// </summary>
        public static string GetMd5(string data, string encode = "utf-8")
        {
            return GetMd5(Encoding.GetEncoding(encode).GetBytes(data));
        }
    }
}
