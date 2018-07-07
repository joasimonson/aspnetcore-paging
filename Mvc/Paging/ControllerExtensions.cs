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
    /// Provides extensions to the controller base.
    /// </summary>
    public static class ControllerExtensions
    {
        /// <summary>
        /// Returns a HTTP 200 OK with a paging result set from an IQueryable.
        /// </summary>
        /// <param name="controller">Base controller.</param>
        /// <param name="source">IQueryable source.</param>
        /// <returns></returns>
        public static PagingResult Paging(this ControllerBase controller, IQueryable source)
        {
            return new PagingResult(source);
        }

        /// <summary>
        /// Returns a HTTP 200 OK with a paging result set from an IQueryable.
        /// </summary>
        /// <typeparam name="T">Collection type of source.</typeparam>
        /// <param name="controller">Base controller.</param>
        /// <param name="source">IQueryable source.</param>
        /// <returns></returns>
        public static PagingResult<T> Paging<T>(this ControllerBase controller, IQueryable<T> source)
        {
            return new PagingResult<T>(source);
        }
    }

}