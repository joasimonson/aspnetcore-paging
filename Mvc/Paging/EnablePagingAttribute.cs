using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Mvc.Paging
{
    /// <summary>
    /// Enable method to return paged results sets.
    /// </summary>
    /// <remarks>Metod must return IQuerable as result.</remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class EnablePagingAttribute : ActionFilterAttribute
    {
        #region === member variables
        private int? _maxPageSize = null; 
        #endregion

        #region === constructor ===
        /// <summary>
        /// 
        /// </summary>
        public EnablePagingAttribute()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxPageSize"></param>
        public EnablePagingAttribute(int maxPageSize)
        {
            _maxPageSize = maxPageSize;
        }
        #endregion

        /// <summary>
        /// Invoked as the method executes, and convert the output IQuerable to a paged result set.
        /// </summary>
        /// <param name="context">Context of the request.</param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var httpContext = context.HttpContext;
            var httpRequest = httpContext.Request;
            var httpResponse = httpContext.Response;
            
            if (httpResponse.IsSuccessStatusCode())
            {
                var pi = PagingInfo.FromRequest(httpRequest);
                var or = (ObjectResult)context.Result;

                var queryableValue = or.Value as IQueryable;
                if (queryableValue == null)
                    throw new ArgumentException("Must return IQueryable from controller.");

                or.Value = queryableValue.ToPagedResult(pi, httpRequest);
                //or.Value = new PagedResult PagingCollection {
                //    Collection = queryableValue.Paginate(pi.Page, pi.PageSize)
                //    , Pagination = new PagingInfo {
                //        Page = pi.Page
                //        , PageSize = pi.PageSize
                //        , MaxPageSizeAllowed = _maxPageSize == null ? 1000 : _maxPageSize.Value
                //        , Result = queryableValue.Counter()
                //    }
                //};
            }
            else
            {
                base.OnActionExecuted(context);
            }
        }        
    }

}
