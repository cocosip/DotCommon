using System;
using System.Text;

namespace DotCommon.Encrypt
{
    /// <summary>Base64加密相关
    /// </summary>
    public static class Base64Helper
    {

        /// <summary>base64编码
        /// </summary>
        public static string Base64Encode(string data, string encoding = "utf-8")
        {
            var dataBytes = Encoding.GetEncoding(encoding).GetBytes(data);
            return Convert.ToBase64String(dataBytes);
        }

        /// <summary>Base64进行解密
        /// </summary>
        public static string Base64Decode(string data, string encoding = "utf-8")
        {
            var bytes = Convert.FromBase64String(data);
            return Encoding.GetEncoding(encoding).GetString(bytes);
        }


        /// <summary>将Base64转换成图片显示的格式
        /// </summary>
        public static string FormatImage(string base64, string header = "image/jpg")
        {
            return $"data:{header};base64,{base64}";
        }
    }
}
