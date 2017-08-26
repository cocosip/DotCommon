using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Alg
{
    /// <summary>SHA1算法
    /// </summary>
    public class Sha1Alg
    {
        #region +获取哈希值，所有的文件或者字符串必须是byte类型
        /// <summary>获取哈希值，所有的文件或者字符串必须是byte类型
        /// </summary>
        public static string GetSha1Hash(byte[] tmpSource)
        {
            //ASCIIEncoding.ASCII.GetBytes(source) 接收到byte编码方式
            var tmpHash = SHA1.Create().ComputeHash(tmpSource);
            return ByteArrayToString(tmpHash);
        }
        /// <summary>从文件路径读取MD5
        /// </summary>
        public static string GetSha1Hash(string source)
        {
            using (var fs = new FileStream(source, FileMode.Open, FileAccess.Read))
            {
                var bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                return GetSha1Hash(bytes);
            }
        }
        /// <summary> 获取字符的Hash值
        /// </summary>
        public static string GetStringSha1(string str, Encoding encode = null)
        {
            encode = encode ?? Encoding.UTF8;
            var bytes = encode.GetBytes(str);
            return GetSha1Hash(bytes);
        }

        /// <summary> 获取字符的Hash值
        /// </summary>
        public static string GetStringSha1(string str, string encode = "utf-8")
        {
            var encoding = Encoding.GetEncoding(encode);
            var bytes = encoding.GetBytes(str);
            return GetSha1Hash(bytes);
        }
        #endregion

        #region +将数据源计算出来的哈希值 存储为一个十六进制的字符串
        private static string ByteArrayToString(byte[] arrInput)
        {
            var sOutput = new StringBuilder(arrInput.Length);
            for (var i = 0; i < arrInput.Length - 1; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }
        #endregion
    }
}
