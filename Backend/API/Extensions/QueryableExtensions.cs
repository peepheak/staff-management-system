using System.Net;
using API.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class QueryableExtensions
{
    public static async Task<PaginatedResult<T>> ToPaginatedAsync<T>(this IQueryable<T> source, int pageNumber,
        int pageSize, HttpStatusCode statusCode) where T : class
    {
        pageNumber = pageNumber == 0 ? 1 : pageNumber;
        pageSize = pageSize == 0 ? 10 : pageSize;
        var count = await source.CountAsync();
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        return PaginatedResult<T>.Success(items, count, pageNumber, pageSize, statusCode);
    }
}