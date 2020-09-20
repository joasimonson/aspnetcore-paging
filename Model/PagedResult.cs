namespace Microsoft.AspNetCore.Mvc
{
    // TODO: recode to PagingContext

    /// <summary>
    /// Generic class of paged results.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedResult<T> where T : class
    {
        /// <summary>Gets or sets the collection.</summary>
        public T[] Collection { get; set; }
        /// <summary>Gets or sets the pagination information.</summary>
        public PagingInfo Pagination { get; set; } 
    }

    /// <summary>
    /// Non-generic class of paged results.
    /// </summary>
    public class PagedResult : PagedResult<object>
    {
    }
}
