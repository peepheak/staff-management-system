using API.Context;
using Microsoft.EntityFrameworkCore;

namespace API.Middleware;

public class DatabaseConnection(RequestDelegate next, ILogger<DatabaseConnection> logger)
{
    public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
    {
        try
        {
            await dbContext.Database.OpenConnectionAsync();
            await dbContext.Database.CloseConnectionAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Database connection failed");

            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            await context.Response.WriteAsync("Database connection unavailable");
            return;
        }

        await next(context);
    }
}