# ASP.NET Core Paging for MVC/API
Enables easy paging of IQueryable collections for ASP.NET Core MVC or APis.
## Getting Started ##
Supports attribute based capture of returns, or by Controller response.
### Attribute based
Return response by attribute using the 'Microsoft.AspNetCore.Mvc.Paging' namespace. The **EnablePagingAttribute** will intercept the call and provide the context class and query inputs.
```
[HttpGet]
[EnablePaging]
public IQueryable<SomeObject> GetAll() {
    return Context.SomeCollection.AsQueryable();
}
```
### Controller response
Return response by controller by using the 'Microsoft.AspNetCore.Mvc' namespace. Get a paged response as follows (example using Entity Framework):
```
[HttpGet]
public IActionResult GetAll() {
    return this.Paging(
        Context.SomeCollection.AsQueryable()
    );
}
```
## Response of API ##
Either implementation will return a formatted page result using the paging information and the collection at hand. The JSON response will become:
```
{
    "Collection" : [...],
    "Paging" : {
        "TotalResults" : 0,
        "TotalPages" : 0,
        "Page" : 0,
        "PageSize" : 0,
        "Previous" : "",
        "Next" : ""
    }
}
```
Pagination of the collection will maximum return 1.000 records per page. The paging block includes the following information:

Property | Type | Description
--- | --- | ---
TotalResults | Integer | Returns a value indicating the total size of the collection non-paged.
TotalPages | Integer | Returns a value indicating the total number of pages (total results / pagesize ).
Page | Integer | Retuns a value indicating the current page (page query parameter).
PageSize | Integer | Returns a value indicating the current results per page.
Previous | String | Returns a link to the previous page if present.
Next | String | Returns a link to the next page if present.
## Controlling the paging ##
You can specify the number of results per page, but applying the 'pagesize' query parameter.