using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DotCommon.Http
{
    public interface ISharpClient
    {

        /// <summary>执行请求
        /// </summary>
        Task<Response> ExecuteAsync(RequestBuilder builder);
    }
}
