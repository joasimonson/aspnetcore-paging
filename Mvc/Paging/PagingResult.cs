using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Paging;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// Represents a paging result.
    /// </summary>
    /// <typeparam name="T">Collection type.</typeparam>
    public class PagingResult<T> : PagingResult
    {
        #region === constructor ===
        /// <summary>
        /// Returns a new instance.
        /// </summary>
        /// <param name="source">Collection source.</param>
        internal PagingResult(IQueryable<T> source) : base(source)
        {
        } 
        #endregion
    }

    /// <summary>
    /// Represents a paging result.
    /// </summary>
    public class PagingResult : IActionResult
    {
        #region === member variables ===
        /// <summary>Source.</summary>
        private IQueryable _source;
        #endregion

        #region === constructor ===
        /// <summary>
        /// Returns a new instance.
        /// </summary>
        /// <param name="source">IQueryable source.</param>
        internal PagingResult(IQueryable source)
        {
            _source = source;
        }
        #endregion

        #region === public methods ===
        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <param name="context">Context of the action request.</param>
        /// <returns></returns>
        public Task ExecuteResultAsync(ActionContext context)
        {
            var queryableValue = _source as IQueryable;
            if (queryableValue == null)
                throw new ArgumentException("Must return IQueryable from controller.");

            var httpContext = context.HttpContext;
            var httpRequest = httpContext.Request;
            var httpResponse = httpContext.Response;

            if (httpResponse.IsSuccessStatusCode())
            {
                var pi = PagingInfo.FromRequest(httpRequest);
                var or = new OkObjectResult(
                        _source.ToPagedResult(pi, httpRequest)
                    );
                return or.ExecuteResultAsync(context);
                //return Task.FromResult(or);
            }
            throw new ArgumentException("Response is not a success.");
        } 
        #endregion
    }
}