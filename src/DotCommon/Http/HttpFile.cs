using System;
using System.IO;

namespace DotCommon.Http
{
    public class HttpFile
    {
        /// <summary>发送的长度
        /// </summary>
        public long ContentLength { get; set; }

        /// <summary> Provides raw data for file
        /// </summary>
        public Action<Stream> Writer { get; set; }

        /// <summary>上传文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>MIME
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// </summary>
        public string Name { get; set; }
    }
}
