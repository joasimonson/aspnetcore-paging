using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// Enable method to return paged results sets.
    /// </summary>
    /// <remarks>Metod must return IQuerable as result.</remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class EnablePagingAttribute : ActionFilterAttribute
    {
        private readonly int? _maxPageSize = null;

        public EnablePagingAttribute()
        {

        }

        public EnablePagingAttribute(int maxPageSize)
        {
            _maxPageSize = maxPageSize;
        }

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

                if (!(or.Value is IQueryable queryableValue))
                    throw new ArgumentException("Must return IQueryable from controller.");

                or.Value = queryableValue.ToPagedResult(pi, httpRequest);
            }
            else
            {
                base.OnActionExecuted(context);
            }
        }
    }
}
