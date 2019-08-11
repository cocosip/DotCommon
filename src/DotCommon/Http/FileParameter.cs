using System;
using System.IO;

namespace DotCommon.Http
{
    /// <summary>文件参数
    /// </summary>
    public class FileParameter
    {
        ///<summary>
        ///根据二进制创建文件参数
        ///</summary>
        ///<param name="name">参数名</param>
        ///<param name="data">二进制数据</param>
        ///<param name="filename">文件名</param>
        ///<param name="contentType">contentType</param>
        ///<returns>The <see cref="FileParameter"/></returns>
        public static FileParameter Create(string name, byte[] data, string filename, string contentType) =>
            new FileParameter
            {
                Writer = s => s.Write(data, 0, data.Length),
                FileName = filename,
                ContentType = contentType,
                ContentLength = data.LongLength,
                Name = name
            };

        ///<summary>
        /// 根据二进制创建文件参数
        ///</summary>
        ///<param name="name">参数名</param>
        ///<param name="data">二进制数据</param>
        ///<param name="filename">文件名</param>
        ///<returns>The <see cref="FileParameter"/> 使用默认的contentType</returns>
        public static FileParameter Create(string name, byte[] data, string filename) =>
            Create(name, data, filename, null);

        /// <summary>
        ///  根据二进制创建文件参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="writer">写文件的委托</param>
        /// <param name="contentLength">contentType</param>
        /// <param name="fileName">文件名</param>
        /// <param name="contentType">Optional: parameter content type</param>
        /// <returns>The <see cref="FileParameter"/> using the default content type.</returns>
        public static FileParameter Create(string name, Action<Stream> writer, long contentLength, string fileName,
            string contentType = null) =>
            new FileParameter
            {
                Name = name,
                FileName = fileName,
                ContentType = contentType,
                Writer = writer,
                ContentLength = contentLength
            };

        /// <summary>发送数据的长度
        /// </summary>
        public long ContentLength { get; set; }

        /// <summary>写二进制的操作
        /// </summary>
        public Action<Stream> Writer { get; set; }

        /// <summary>上传文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>文件MIME content type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>参数名
        /// </summary>
        public string Name { get; set; }
    }
}
