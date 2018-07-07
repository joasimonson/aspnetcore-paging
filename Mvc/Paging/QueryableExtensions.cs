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
    /// Provides extensions to IQueryable.
    /// </summary>
    public static class QueryableExtensions
    {        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pi"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static PagedResult ToPagedResult(this IQueryable source, PagingInfo pi, Http.HttpRequest request = null)
        {
            var list = GetGenericSkip((dynamic)source, (pi.Page - 1) * pi.PageSize, pi.PageSize);

            int totalResults = GetGenericCount((dynamic)source);
            int totalPages = (int)Math.Ceiling( (decimal)totalResults / (decimal)pi.PageSize );

            string nextLink = ""; string previousLink = "";

            if (request != null)
            {
                if (pi.Page < totalPages)
                {
                    nextLink = request.SchemeAndHost();
                    nextLink += "?page=" + (pi.Page + 1).ToString();
                    if (request.Query.ContainsKey("pagesize"))
                        nextLink += "&pagesize=" + request.Query["pagesize"];
                }
                if (pi.Page > 1)
                {
                    previousLink = request.SchemeAndHost();
                    previousLink += "?page=" + (pi.Page - 1).ToString();
                    if (request.Query.ContainsKey("pagesize"))
                        previousLink += "&pagesize=" + request.Query["pagesize"];
                }
            }

            return new PagedResult()
            {
                Collection = list,
                Pagination = new PagingInfo {
                    TotalResults = totalResults // total number of records returned, i.e. 4300
                    , TotalPages = totalPages // total number of pages (ceiling) = totalResults / MaxPageSize
                    , PageSize = pi.PageSize // current page size, would be 3 x 1000 then 1 x 300
                    , Page = pi.Page // current page
                    //, MaxPageSizeAllowed = pi.MaxPageSizeAllowed
                    , Next = nextLink
                    , Previous = previousLink
                }
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        private static int GetGenericCount<T>(IQueryable<T> source)
        {
            return source.Count();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        private static T[] GetGenericSkip<T>
                (IQueryable<T> source, int skip, int take)
        {
            return source.Skip(skip).Take(take).ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pi"></param>
        /// <returns></returns>
        public static PagedResult<T> ToPagedResult<T>(this IQueryable<T> source, PagingInfo pi) where T : class
        {
            return new PagedResult<T>() {
                Collection = source.Paginate<T>(pi).ToArray(),
                Pagination = pi
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public static IQueryable<T> Paginate<T>(this IQueryable<T> source, PagingInfo pagination) where T : class
        {
            return source
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize);
        }
    }

}