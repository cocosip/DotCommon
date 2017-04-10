using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Alg
{
    /// <summary>哈希值计算
    /// </summary>
    public class HashAlg
    {
        #region +获取哈希值，所有的文件或者字符串必须是byte类型
        public static string GetMd5Hash(byte[] tmpSource)
        {
            //ASCIIEncoding.ASCII.GetBytes(source) 接收到byte编码方式
            var tmpHash = MD5.Create().ComputeHash(tmpSource);
            return ByteArrayToString(tmpHash);
        }
        //从文件路径读取MD5
        public static string GetMd5Hash(string source)
        {
            using (var fs = new FileStream(source, FileMode.Open, FileAccess.Read))
            {
                var bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                return GetMd5Hash(bytes);
            }
        }
        //从文字中获取hash
        public static  string GetStringHash(string str, Encoding code = default(Encoding))
        {
            var encode = code ?? Encoding.UTF8;
            var bytes = encode.GetBytes(str);
            return GetMd5Hash(bytes);
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
