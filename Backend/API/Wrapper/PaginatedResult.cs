using System.Net;
using API.Constants;

namespace API.Wrapper;

public class PaginatedResult<T> : Result
{
    public PaginatedResult(List<T> data)
    {
        Data = data;
    }

    private PaginatedResult(
        bool isSuccess,
        List<T>? data,
        List<string>? messages,
        int count = 0,
        int page = 1,
        int pageSize = 10,
        HttpStatusCode status = default)
    {
        Data = data;
        CurrentPage = page;
        PageSize = pageSize;
        TotalCount = count;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        IsSuccess = isSuccess;
        Status = status;
        Message = data?.Count > 0 ? "Success" : string.Join(',', messages ?? new List<string>());
    }

    public List<T>? Data { get; set; }

    private int CurrentPage { get; set; }

    private int TotalPages { get; set; }

    public int TotalCount { get; private set; }

    public int PageSize { get; private set; }

    public bool HasNextPage => CurrentPage < TotalPages;

    public bool HasPreviousPage => CurrentPage > 1;

    public new HttpStatusCode Status { get; set; }

    public static PaginatedResult<T> Failure(List<string> messages, HttpStatusCode status = default)
    {
        return new PaginatedResult<T>(false, null, messages, 0, 1, 10, status);
    }

    public static PaginatedResult<T> Success(
        List<T> data,
        int count,
        int page,
        int pageSize,
        HttpStatusCode status = default)
    {
        return new PaginatedResult<T>(true, data, [ApplicationConstants.Message.Recieved], count, page, pageSize,
            status);
    }
}