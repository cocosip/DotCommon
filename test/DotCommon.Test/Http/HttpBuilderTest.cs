using DotCommon.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace DotCommon.Test.Http
{
    public class HttpBuilderTest
    {

        [Fact]
        public void Parameter_Test()
        {
            var p1 = new Parameter("n1", 1, ParameterType.Cookie);
            var p2 = new Parameter("n2", "v2", ParameterType.GetOrPost);
            var p3 = new Parameter()
            {
                Name = "n3",
                Value = 10.2m,
                ContentType = "application/json",
                DataFormat = DataFormat.Json,
                Type = ParameterType.HttpHeader
            };

            var p4 = new Parameter("n4", "v4", "application/xml", ParameterType.QueryString);


            Assert.Equal(1, p1.Value);
            Assert.Equal("n1=1", p1.ToString());
            Assert.Equal("v2", p2.Value);
            Assert.Equal("application/json", p3.ContentType);
            Assert.Equal(ParameterType.HttpHeader, p3.Type);
            Assert.Equal(DataFormat.Json, p3.DataFormat);
            Assert.Equal(ParameterType.QueryString, p4.Type);
        }

        [Fact]
        public void FileParameter_Test()
        {
            var f1 = FileParameter.Create("name1", new byte[] { 1 }, "f1.txt");
            var f2 = FileParameter.Create("name2", new byte[] { 1 }, "f2.txt", "application/json");
            var f3 = FileParameter.Create("name3", x => { }, 100, "f3.txt", "application/xml");


            Assert.Null(f1.ContentType);
            Assert.Equal(1, f1.ContentLength);

            Assert.Equal("application/json", f2.ContentType);
            Assert.Equal("name2", f2.Name);
            Assert.Equal("f2.txt", f2.FileName);

            Assert.NotNull(f3.Writer);

        }

        [Fact]
        public void HttpBuilder_Test()
        {
            var httpBuilder = new HttpBuilder("/user", Method.GET, DataFormat.None);
            httpBuilder.Parameters = new List<Parameter>()
            {
                new Parameter("px","px",ParameterType.UrlSegment)
            };

            httpBuilder.AddParameter(new Parameter("p1", "v1", ParameterType.HttpHeader));
            httpBuilder.AddParameter("p2", 22);
            httpBuilder.AddParameter("p3", 33m, ParameterType.QueryString);
            httpBuilder.AddFile("f1", "DotCommon.dll", "f1/contentType");
            httpBuilder.AddFile("f2", new byte[] { 1 }, "f2.txt", "f2/contentType");
            httpBuilder.AddFile("f3", w => { }, "f3.txt", 1, "f3/contentType");
            httpBuilder.AddFile(new FileParameter()
            {
                Name = "f4",
                FileName = "f4.txt",
                ContentLength = 101,
                ContentType = "f4/contentType",
            });

            Assert.Contains(httpBuilder.Parameters, x => x.Name == "p1");
            Assert.Equal(Method.GET, httpBuilder.Method);
            Assert.Equal("/user", httpBuilder.Resource);
            Assert.Equal(4, httpBuilder.Files.Count);
            Assert.Equal(4, httpBuilder.Parameters.Count);
            Assert.Equal(DataFormat.None, httpBuilder.DataFormat);
        }
    }
}
