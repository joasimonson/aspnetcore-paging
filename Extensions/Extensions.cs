using Microsoft.AspNetCore.Http;
using System;

namespace Microsoft.AspNetCore
{
    /// <summary>
    /// Provides shared extensions for this project.
    /// </summary>
    static class Extensions
    {
        /// <summary>
        /// Returns true if the HttpResponse is a 2xx response code.
        /// </summary>
        /// <param name="response">HttpResponse object.</param>
        /// <returns></returns>
        public static bool IsSuccessStatusCode(this HttpResponse response)
        {
            if (response.StatusCode >= 200 && response.StatusCode < 299)
                return true;

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string SchemeAndHost(this Http.HttpRequest request, string appendQuery = null)
        {
            return string.Format("{0}://{1}{2}", request.Scheme, request.Host, request.Path);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T TryGet<T>(this IQueryCollection query, string key)
        {
            if (query.ContainsKey(key))
            {
                if (query.TryGetValue(key, out Microsoft.Extensions.Primitives.StringValues keyValue))
                    return (T)Convert.ChangeType(keyValue.ToString(), typeof(T));
            }
            throw new ArgumentNullException();
        }
    }
}
