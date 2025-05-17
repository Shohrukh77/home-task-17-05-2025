using System.Net;

namespace Domain.Responses;

public class PagedResponse<T> : Response<T>
{
    public int PageNumber { get; set; } 
    public int PageSize { get; set; }
    public int TotalPages { get; set; } 
    public int TotalRecords { get; set; } 

    public PagedResponse(T data, int pageNumber, int pageSize, int totalRecords) : base(data)
    {
        PageNumber = pageNumber; // 1
        PageSize = pageSize; // 50
        TotalRecords = totalRecords; // 351
        TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize); // 351 / 50 = 8
    }

    public PagedResponse(HttpStatusCode statusCode, string message) : base(statusCode, message)
    {
        
    }
}
