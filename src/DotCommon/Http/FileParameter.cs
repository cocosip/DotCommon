using System;
using System.IO;

namespace DotCommon.Http
{
    public class FileParameter
    {
        public long ContentLength { get; set; }

        public Action<Stream> Writer { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public string Name { get; set; }

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

        public static FileParameter Create(string name, byte[] data, string filename)
        {
            return Create(name, data, filename, null);
        }
    }
}
