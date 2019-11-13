using System;
using System.Collections.Generic;
using System.IO;

namespace DotCommon.Http
{
    /// <summary>Http请求构建
    /// </summary>
    public class HttpBuilder
    {
        /// <summary>Resource资源
        /// </summary>
        public string Resource { get; set; }

        /// <summary>方法
        /// </summary>
        public Method Method { get; set; }

        /// <summary>数据格式
        /// </summary>
        public DataFormat DataFormat { get; set; } = DataFormat.None;

        /// <summary>MIME content type of the parameter
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>参数
        /// </summary>
        public List<Parameter> Parameters { get; set; } = new List<Parameter>();

        /// <summary>上传的文件
        /// </summary>
        public List<FileParameter> Files { get; } = new List<FileParameter>();

        /// <summary>Ctor
        /// </summary>
        public HttpBuilder()
        {

        }

        /// <summary>Ctor
        /// </summary>
        public HttpBuilder(Method method) : this()
        {
            Method = method;
        }


        /// <summary>Ctor
        /// </summary>
        public HttpBuilder(string resource, Method method) : this(method)
        {
            Resource = resource;
        }

        /// <summary>Ctor
        /// </summary>
        public HttpBuilder(string resource, Method method, DataFormat dataFormat) : this(resource, method)
        {
            DataFormat = dataFormat;
        }

        /// <summary>添加参数
        /// </summary>
        public HttpBuilder AddParameter(Parameter p)
        {
            Parameters.Add(p);
            return this;
        }

        /// <summary>添加参数
        /// </summary>
        public HttpBuilder AddParameter(string name, object value) => AddParameter(new Parameter(name, value, ParameterType.GetOrPost));

        /// <summary>添加参数
        /// </summary>
        public HttpBuilder AddParameter(string name, object value, ParameterType type)
         => AddParameter(new Parameter(name, value, type));

        /// <summary>添加参数
        /// </summary>
        public HttpBuilder AddParameter(string name, object value, string contentType, ParameterType type)
         => AddParameter(new Parameter(name, value, contentType, type));

        /// <summary>添加Json参数
        /// </summary>
        public HttpBuilder AddJsonBody(object value) => AddParameter(new Parameter("", value, ContentTypeConsts.Json, ParameterType.RequestBody)
        {
            DataFormat = DataFormat.Json
        });

        /// <summary>添加Xml参数
        /// </summary>
        public HttpBuilder AddXmlBody(object value) => AddParameter(new Parameter("", value, ContentTypeConsts.Xml, ParameterType.RequestBody)
        {
            DataFormat = DataFormat.Json
        });

        /// <summary>添加文件
        /// </summary>
        public HttpBuilder AddFile(FileParameter file)
        {
            Files.Add(file);
            return this;
        }

        /// <summary>添加文件
        /// </summary>
        public HttpBuilder AddFile(string name, string path, string contentType = null)
        {
            var f = new FileInfo(path);
            var fileLength = f.Length;

            return AddFile(new FileParameter
            {
                Name = name,
                FileName = Path.GetFileName(path),
                ContentLength = fileLength,
                Writer = s =>
                {
                    var file = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read));
                    file.BaseStream.CopyTo(s);
                },
                ContentType = contentType
            });
        }

        /// <summary>添加文件
        /// </summary>
        public HttpBuilder AddFile(string name, byte[] bytes, string fileName, string contentType = null)
          => AddFile(FileParameter.Create(name, bytes, fileName, contentType));

        /// <summary>添加文件
        /// </summary>
        public HttpBuilder AddFile(string name, Action<Stream> writer, string fileName, long contentLength, string contentType = null) => AddFile(new FileParameter
        {
            Name = name,
            Writer = writer,
            FileName = fileName,
            ContentLength = contentLength,
            ContentType = contentType
        });

    }
}
