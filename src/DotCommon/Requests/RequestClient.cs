using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DotCommon.Requests
{
    public class RequestClient : IRequestClient
    {
        private readonly HttpClient _client;
        private readonly ClientConfig _config;
        private readonly object _syncObject = new object();
        /// <summary>存放SSL请求的地址
        /// </summary>
        private readonly ConcurrentDictionary<string, string> _sslRequest = new ConcurrentDictionary<string, string>();

        public RequestClient() : this(null)
        {

        }

        public RequestClient(ClientConfig config)
        {
            if (_config == null)
            {
                _config = new ClientConfig();
            }
            if (_client == null)
            {
                _client = CreateClient();
            }
        }

        /// <summary>根据ClientConfig创建HttpClient
        /// </summary>
        private HttpClient CreateClient()
        {
            var handler = ParseHandler(_config);
            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Connection.Add(RequestConsts.KeepAlive);
            return client;
        }

#if NETSTANDARD2_0
        private HttpClientHandler ParseHandler(ClientConfig config)
        {
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = config.AllowAutoRedirect,
                // MaxRequestContentBufferSize = config.MaxRequestContentBufferSize,
                MaxAutomaticRedirections = config.MaxAutomaticRedirections
            };
            //如果使用SSL则需要添加证书
            if (config.IsSsl)
            {
                if (config.Cers.Any())
                {
                    foreach (var cer in config.Cers)
                    {
                        handler.ClientCertificates.Add(cer);
                    }
                }
                handler.ServerCertificateCustomValidationCallback += (a, b, c, d) => true;
            }
            return handler;
        }
#else
        private WebRequestHandler ParseHandler(ClientConfig config)
        {
            var handler = new WebRequestHandler()
            {
                AllowAutoRedirect = config.AllowAutoRedirect,
                MaxRequestContentBufferSize = config.MaxRequestContentBufferSize,
                MaxAutomaticRedirections = config.MaxAutomaticRedirections
            };
            //如果使用SSL则需要添加证书
            if (config.IsSsl)
            {
                if (config.Cers.Any())
                {
                    foreach (var cer in config.Cers)
                    {
                        handler.ClientCertificates.Add(cer);
                    }
                }
                handler.ServerCertificateValidationCallback = (a, b, c, d) => true;
            }
            return handler;
        }
#endif

        private void CheckSslRequest(RequestOptions options)
        {
            if (options.Url.StartsWith("https") && options.IsSsl && options.Cer != null)
            {
                var host = new Uri(options.Url).Host;
                //已经包含该url
                if (!_sslRequest.ContainsKey(host))
                {
                    lock (_syncObject)
                    {
                        _config.IsSsl = true;
                        _config.Cers.Add(options.Cer);
                        //创建Client
                        CreateClient();
                    }
                }
            }
        }

        /// <summary>异步发送请求
        /// </summary>
        public async Task<Response> SendAsync(RequestBuilder builder)
        {
            try
            {
                var options = builder.GetOptions();
                CheckSslRequest(options);
                var message = RequestUtil.BuildRequestMessage(options);
                var response = await _client.SendAsync(message);
                return await RequestUtil.ParseResponse(response);
            }
            catch (AggregateException ex)
            {
                return RequestUtil.BuildErrorResponse(ex);
            }
            catch (Exception ex)
            {
                return RequestUtil.BuildErrorResponse(ex);
            }
        }

    }
}
