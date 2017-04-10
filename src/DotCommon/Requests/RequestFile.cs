using System.IO;

namespace DotCommon.Requests
{
    public class RequestFile
    {
        public string ParamName { get; set; }
        public string FileName { get; set; }
        public Stream Data { get; set; }
    }
}
