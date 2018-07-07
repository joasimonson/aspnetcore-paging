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
    /// /
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagingResult<T> : PagingResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        public PagingResult(IQueryable<T> source) : base(source)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PagingResult : IActionResult
    {
        private IQueryable _source;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        public PagingResult(IQueryable source)
        {
            _source = source;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
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
            throw new ArgumentException();
        }
    }

}