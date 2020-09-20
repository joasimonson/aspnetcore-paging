using System;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// Represents a paging result.
    /// </summary>
    /// <typeparam name="T">Collection type.</typeparam>
    public class PagingResult<T> : PagingResult
    {
        /// <summary>
        /// Returns a new instance.
        /// </summary>
        /// <param name="source">Collection source.</param>
        internal PagingResult(IQueryable<T> source) : base(source)
        {
        } 
    }

    /// <summary>
    /// Represents a paging result.
    /// </summary>
    public class PagingResult : IActionResult
    {
        private readonly IQueryable _source;

        /// <summary>
        /// Returns a new instance.
        /// </summary>
        /// <param name="source">IQueryable source.</param>
        internal PagingResult(IQueryable source)
        {
            _source = source;
        }

        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <param name="context">Context of the action request.</param>
        /// <returns></returns>
        public Task ExecuteResultAsync(ActionContext context)
        {
            if (!(_source is IQueryable))
                throw new ArgumentException("Must return IQueryable from controller.");

            var httpContext = context.HttpContext;
            var httpRequest = httpContext.Request;
            var httpResponse = httpContext.Response;

            if (!httpResponse.IsSuccessStatusCode())
            {
                throw new ArgumentException("Response is not a success.");
            }

            var pi = PagingInfo.FromRequest(httpRequest);
            var or = new OkObjectResult(_source.ToPagedResult(pi, httpRequest));
            return or.ExecuteResultAsync(context);
        } 
    }
}