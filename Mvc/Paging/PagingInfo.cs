using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Microsoft.AspNetCore.Mvc.Paging
{
    /// <summary>
    /// Provides properties for a paged result set, including the page size and results.
    /// </summary>
    public sealed class PagingInfo
    {
        #region === constructor ===
        /// <summary>
        /// Returns a new instance of the paging info object.
        /// </summary>
        internal PagingInfo()
        {
            this.Page = 1;
            this.PageSize = 50;
            this.TotalResults = 0;
            this.MaxPageSizeAllowed = 1000;
            this.Next = null;
            this.Previous = null;
        }
        #endregion

        #region === properties ===
        /// <summary>Gets or sets the results per page.</summary>
        public int PageSize { get; set; }
        /// <summary>Gets or sets the total number of results</summary>
        public int TotalResults { get; set; }
        public int TotalPages { get; set; }
        /// <summary>Gets or sets the maximum results per page.</summary>
        private int MaxPageSizeAllowed { get; set; }
        /// <summary>Gets or sets the current page.</summary>
        public int Page { get; set; }
        /// <summary>Gets a link to the previous page.</summary>
        public string Previous { get; internal set; }
        /// <summary>Gets a link to the next page.</summary>
        public string Next { get; internal set; }
        #endregion

        #region === internal methods ===
        /// <summary>
        /// Returns a paginginfo class from an HttpRequest.
        /// </summary>
        /// <param name="httpRequest">Request to retrieve paginig info from.</param>
        /// <returns></returns>
        public static PagingInfo FromRequest(HttpRequest httpRequest)
        {
            PagingInfo retValue = new PagingInfo();
            if (httpRequest.Query.ContainsKey("page"))
                retValue.Page = httpRequest.Query.TryGet<int>("page");
            if (httpRequest.Query.ContainsKey("pagesize"))
                retValue.PageSize = httpRequest.Query.TryGet<int>("pagesize");

            if (retValue.PageSize > 1000)
                throw new ArgumentOutOfRangeException("PageSize must be 1000 or below");

            return retValue;
        } 
        #endregion
    }
}