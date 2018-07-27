using System;
using System.IO;

namespace DotCommon.Http
{
    public class FileParameter
    {
        /// <summary>创建文件
        /// </summary>
        public static FileParameter Create(string name, byte[] data, string filename, string contentType)
        {
            long length = data.LongLength;

            return new FileParameter
            {
                Writer = s => s.Write(data, 0, data.Length),
                FileName = filename,
                ContentType = contentType,
                ContentLength = length,
                Name = name
            };
        }

        /// <summary>创建文件
        /// </summary>
        public static FileParameter Create(string name, byte[] data, string filename)
        {
            return Create(name, data, filename, null);
        }

        /// <summary>文件长度
        /// </summary>
        public long ContentLength { get; set; }

        /// <summary>Provides raw data for file
        /// </summary>
        public Action<Stream> Writer { get; set; }

        /// <summary>上传的文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>文件的MIME名
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>参数名称
        /// </summary>
        public string Name { get; set; }
    }
}
