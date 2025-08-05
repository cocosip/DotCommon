using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace DotCommon.Utility
{
    /// <summary>
    /// Provides utility methods for URL manipulation and validation.
    /// </summary>
    public static class UrlUtil
    {
        /// <summary>
        /// Determines whether the specified URL uses the HTTPS protocol.
        /// </summary>
        /// <param name="url">The URL to check.</param>
        /// <returns>True if the URL uses the HTTPS protocol; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when url is null.</exception>
        /// <exception cref="UriFormatException">Thrown when url is not a valid URI.</exception>
        public static bool IsHttps(string url)
        {
            if (url == null)
                throw new ArgumentNullException(nameof(url));
                
            var uri = new Uri(url);
            return uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the host portion of the URL including scheme and port.
        /// </summary>
        /// <param name="url">The URL to process.</param>
        /// <returns>The host URL with scheme and port.</returns>
        /// <exception cref="ArgumentNullException">Thrown when url is null.</exception>
        /// <exception cref="UriFormatException">Thrown when url is not a valid URI.</exception>
        public static string GetHostUrl(string url)
        {
            if (url == null)
                throw new ArgumentNullException(nameof(url));
                
            var uri = new Uri(url);
            var builder = new UriBuilder()
            {
                Scheme = uri.Scheme,
                Host = uri.Host,
                Port = uri.Port
            };
            return builder.ToString();
        }

        /// <summary>
        /// Parses and normalizes the URL.
        /// </summary>
        /// <param name="url">The URL to parse.</param>
        /// <returns>The normalized URL.</returns>
        /// <exception cref="ArgumentNullException">Thrown when url is null.</exception>
        /// <exception cref="UriFormatException">Thrown when url is not a valid URI.</exception>
        public static string ParseUrl(string url)
        {
            if (url == null)
                throw new ArgumentNullException(nameof(url));
                
            var builder = new UriBuilder(url);
            return builder.ToString();
        }

        /// <summary>
        /// Determines whether the specified URL is a valid main domain or www domain.
        /// </summary>
        /// <param name="url">The URL to check.</param>
        /// <returns>True if the URL is a valid main domain or www domain; otherwise, false.</returns>
        public static bool IsMainDomain(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            url = url.ToLowerInvariant();
            var reg = new Regex(
                @"^http(s)?\://((www\.)?[a-zA-Z0-9\-]+\.((com|edu|gov|int|mil|net|cn|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10})))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?'\\+&%\$#\-]+))*$");
            return reg.IsMatch(url);
        }

        /// <summary>
        /// Ensures the URL has a scheme prefix (http:// or https://).
        /// </summary>
        /// <param name="url">The URL to process.</param>
        /// <param name="schema">The schema to use if none exists (default is "http://").</param>
        /// <returns>The URL with a scheme prefix.</returns>
        public static string EnsureSchemePrefix(string url, string schema = "http://")
        {
            if (url == null)
                return string.Empty;
                
            url = url.ToLowerInvariant();
            if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && 
                !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return string.Concat(schema, url);
            }
            return url;
        }

        /// <summary>
        /// Gets the authority component of the URL (host and port).
        /// </summary>
        /// <param name="url">The URL to process.</param>
        /// <returns>The authority component of the URL.</returns>
        /// <exception cref="ArgumentNullException">Thrown when url is null.</exception>
        /// <exception cref="UriFormatException">Thrown when url is not a valid URI.</exception>
        public static string GetAuthority(string url)
        {
            if (url == null)
                throw new ArgumentNullException(nameof(url));
                
            return new Uri(url).Authority;
        }

        /// <summary>
        /// Combines multiple URL path segments into a single URL path.
        /// </summary>
        /// <param name="paths">The URL path segments to combine.</param>
        /// <returns>The combined URL path.</returns>
        public static string CombineUrlPaths(params string[] paths)
        {
            if (paths == null || paths.Length == 0)
                return string.Empty;

            var urlBuilder = new StringBuilder();
            for (int i = 0; i < paths.Length; i++)
            {
                if (paths[i] == null)
                    continue;
                    
                paths[i] = paths[i].Replace("../", "").Replace("~/", "");
                if (!paths[i].EndsWith("/"))
                {
                    paths[i] = $"{paths[i]}/";
                }
                if (paths[i].StartsWith("/"))
                {
                    paths[i] = paths[i].Substring(1);
                }
                urlBuilder.Append(paths[i]);
            }
            
            if (urlBuilder.Length > 0 && urlBuilder[urlBuilder.Length - 1] == '/')
            {
                urlBuilder.Remove(urlBuilder.Length - 1, 1);
            }
            
            return urlBuilder.ToString();
        }

        /// <summary>
        /// Determines whether two URLs have the same domain.
        /// </summary>
        /// <param name="url1">The first URL.</param>
        /// <param name="url2">The second URL.</param>
        /// <returns>True if both URLs have the same domain; otherwise, false.</returns>
        public static bool HasSameDomain(string url1, string url2)
        {
            if (string.IsNullOrWhiteSpace(url1) || string.IsNullOrWhiteSpace(url2))
                return false;

            url1 = EnsureSchemePrefix(url1);
            url2 = EnsureSchemePrefix(url2);
            var uri1 = new Uri(url1);
            var uri2 = new Uri(url2);
            
            return string.Equals(
                uri1.Authority.ToLowerInvariant(), 
                uri2.Authority.ToLowerInvariant(), 
                StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Extracts all parameters from the URL's query string.
        /// </summary>
        /// <param name="url">The URL to process.</param>
        /// <returns>A dictionary containing all URL parameters.</returns>
        /// <exception cref="ArgumentNullException">Thrown when url is null.</exception>
        /// <exception cref="UriFormatException">Thrown when url is not a valid URI.</exception>
        public static Dictionary<string, string> ExtractQueryParameters(string url)
        {
            if (url == null)
                throw new ArgumentNullException(nameof(url));
                
            var uri = new Uri(url);
            if (string.IsNullOrEmpty(uri.Query))
                return new Dictionary<string, string>();
                
            var query = uri.Query.StartsWith("?") ? uri.Query.Substring(1) : uri.Query;
            var paramArray = query.Split('&');
            
            return paramArray
                .Select(param => param.Split('='))
                .Where(itemArray => itemArray.Length == 2 && 
                    !string.IsNullOrWhiteSpace(itemArray[0]) && 
                    !string.IsNullOrWhiteSpace(itemArray[1]))
                .ToDictionary(
                    itemArray => itemArray[0], 
                    itemArray => WebUtility.UrlDecode(itemArray[1]));
        }

        /// <summary>
        /// Gets URL parameters excluding the specified ones.
        /// </summary>
        /// <param name="url">The URL to process.</param>
        /// <param name="excludedParams">The parameter names to exclude.</param>
        /// <returns>A dictionary containing URL parameters excluding the specified ones.</returns>
        public static Dictionary<string, string> GetExcludedUrlParameters(string url, params string[] excludedParams)
        {
            var parameters = ExtractQueryParameters(url);
            if (excludedParams == null || excludedParams.Length == 0)
                return parameters;
                
            var lowerExcludedParams = excludedParams.Select(x => x.ToLowerInvariant()).ToList();
            return parameters
                .Where(x => !lowerExcludedParams.Contains(x.Key.ToLowerInvariant()))
                .ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Appends a single parameter to the URL.
        /// </summary>
        /// <param name="url">The URL to process.</param>
        /// <param name="key">The parameter key.</param>
        /// <param name="value">The parameter value.</param>
        /// <param name="replaceExisting">Whether to replace the parameter if it already exists.</param>
        /// <returns>The URL with the appended parameter.</returns>
        public static string AddQueryParameter(string url, string key, string value, bool replaceExisting = false)
        {
            return AddQueryParameters(url, new Dictionary<string, string>() { { key, value } }, replaceExisting);
        }

        /// <summary>
        /// Appends multiple parameters to the URL.
        /// </summary>
        /// <param name="url">The URL to process.</param>
        /// <param name="parameters">The parameters to append.</param>
        /// <param name="replaceExisting">Whether to replace parameters that already exist.</param>
        /// <returns>The URL with the appended parameters.</returns>
        public static string AddQueryParameters(string url, Dictionary<string, string> parameters, bool replaceExisting = false)
        {
            if (url == null)
                throw new ArgumentNullException(nameof(url));
                
            if (parameters == null || parameters.Count == 0)
                return url;

            var existingParameters = ExtractQueryParameters(url);
            var uri = new Uri(url);
            
            // Merge parameters
            foreach (var kv in parameters)
            {
                if (string.IsNullOrWhiteSpace(kv.Key) || string.IsNullOrWhiteSpace(kv.Value))
                    continue;
                    
                var key = kv.Key;
                var encodedValue = WebUtility.UrlEncode(kv.Value);
                
                if (existingParameters.ContainsKey(key))
                {
                    if (replaceExisting)
                    {
                        existingParameters[key] = encodedValue;
                    }
                }
                else
                {
                    existingParameters.Add(key, encodedValue);
                }
            }
            
            // Build the new query string
            var query = existingParameters.Any() ? 
                "?" + string.Join("&", existingParameters.Select(x => $"{x.Key}={x.Value}")) : 
                string.Empty;
                
            var uriBuilder = new UriBuilder()
            {
                Host = uri.Host,
                Scheme = uri.Scheme,
                Fragment = uri.Fragment,
                Path = uri.LocalPath,
                Query = query
            };

            return uriBuilder.Uri.ToString();
        }
    }
}