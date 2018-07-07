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
    /// 
    /// </summary>
    public static class ControllerExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static PagingResult Paging(this ControllerBase controller, IQueryable source)
        {
            return new PagingResult(source);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static PagingResult<T> Paging<T>(this ControllerBase controller, IQueryable<T> source)
        {
            return new PagingResult<T>(source);
        }
    }

}