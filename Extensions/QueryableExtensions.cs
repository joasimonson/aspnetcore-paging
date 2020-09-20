using System;
using System.Linq;

namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// Provides extensions to IQueryable.
    /// </summary>
    public static class QueryableExtensions
    {
        public static PagedResult ToPagedResult(this IQueryable source, PagingInfo pageInfo, Http.HttpRequest request = null)
        {
            return ToPagedResult<object>(source, pageInfo, request) as PagedResult;
        }

        /// <summary>
        /// Returns a paged result from a queryable collection.
        /// </summary>
        /// <param name="source">Collection source.</param>
        /// <param name="pageInfo">Page information.</param>
        /// <param name="request">Http request.</param>
        /// <returns></returns>
        public static PagedResult<T> ToPagedResult<T>(this IQueryable source, PagingInfo pageInfo, Http.HttpRequest request = null) where T : class
        {
            var list = GetGenericSkip((dynamic)source, (pageInfo.Page - 1) * pageInfo.PageSize, pageInfo.PageSize);

            int totalResults = GetGenericCount((dynamic)source);
            int totalPages = (int)Math.Ceiling((decimal)totalResults / (decimal)pageInfo.PageSize);

            string nextLink = ""; string previousLink = "";

            if (request != null)
            {
                if (pageInfo.Page < totalPages)
                {
                    nextLink = request.SchemeAndHost();
                    nextLink += "?page=" + (pageInfo.Page + 1).ToString();
                    if (request.Query.ContainsKey("pagesize"))
                        nextLink += "&pagesize=" + request.Query["pagesize"];
                }
                if (pageInfo.Page > 1)
                {
                    previousLink = request.SchemeAndHost();
                    previousLink += "?page=" + (pageInfo.Page - 1).ToString();
                    if (request.Query.ContainsKey("pagesize"))
                        previousLink += "&pagesize=" + request.Query["pagesize"];
                }
            }

            return new PagedResult<T>()
            {
                Collection = list,
                Pagination = new PagingInfo
                {
                    TotalResults = totalResults, // total number of records returned, i.e. 4300
                    TotalPages = totalPages, // total number of pages (ceiling) = totalResults / MaxPageSize
                    PageSize = pageInfo.PageSize, // current page size, would be 3 x 1000 then 1 x 300
                    Page = pageInfo.Page, // current page
                    //MaxPageSizeAllowed = pi.MaxPageSizeAllowed,
                    Next = nextLink,
                    Previous = previousLink
                }
            };
        }

        /// <summary>
        /// Returns a paged result from a queryable collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">Collection source.</param>
        /// <param name="pageInfo">Page information.</param>
        /// <returns></returns>
        public static PagedResult<T> ToPagedResult<T>(this IQueryable<T> source, PagingInfo pageInfo) where T : class
        {
            return new PagedResult<T>()
            {
                Collection = source.Paginate<T>(pageInfo).ToArray(),
                Pagination = pageInfo
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">Collection source.</param>
        /// <param name="pageInfo">Page information.</param>
        /// <returns></returns>
        public static IQueryable<T> Paginate<T>(this IQueryable<T> source, PagingInfo oageInfo) where T : class
        {
            return source
                .Skip((oageInfo.Page - 1) * oageInfo.PageSize)
                .Take(oageInfo.PageSize);
        } 

        /// <summary>
        /// Returns a count from a generic iqueryable collection.
        /// </summary>
        /// <typeparam name="T">Type of collection.</typeparam>
        /// <param name="source">Collection source.</param>
        /// <returns></returns>
        private static int GetGenericCount<T>(IQueryable<T> source)
        {
            return source.Count();
        }

        /// <summary>
        /// Returns a skip from a generic iqueryable collection.
        /// </summary>
        /// <typeparam name="T">Type of collection.</typeparam>
        /// <param name="source">Collection source.</param>
        /// <param name="skip">Skip this amount.</param>
        /// <param name="take">Take this amount.</param>
        /// <returns></returns>
        private static T[] GetGenericSkip<T>(IQueryable<T> source, int skip, int take)
        {
            return source.Skip(skip).Take(take).ToArray();
        } 
    }
}