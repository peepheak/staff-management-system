using System.Net;

namespace API.Wrapper;

public class Result : IResult
{
    public HttpStatusCode Status { get; set; }
    public string Message { get; set; } = string.Empty;

    public bool IsSuccess { get; set; }

    private static IResult Fail()
    {
        return new Result { IsSuccess = false };
    }

    private static IResult Fail(string message)
    {
        return new Result { IsSuccess = false, Message = message };
    }

    public static Task<IResult> FailAsync()
    {
        return Task.FromResult(Fail());
    }

    public static Task<IResult> FailAsync(string message)
    {
        return Task.FromResult(Fail(message));
    }

    private static IResult Success()
    {
        return new Result { IsSuccess = true };
    }

    private static IResult Success(string message)
    {
        return new Result { IsSuccess = true, Message = message };
    }

    public static Task<IResult> SuccessAsync()
    {
        return Task.FromResult(Success());
    }

    public static Task<IResult> SuccessAsync(string message)
    {
        return Task.FromResult(Success(message));
    }
}

public class Result<T> : Result, IResult<T>
{
    public T? Data { get; set; }

    private static Result<T> Fail(HttpStatusCode status = default)
    {
        return new Result<T> { IsSuccess = false, Status = status };
    }

    private static Result<T> Fail(string message, HttpStatusCode status = default)
    {
        return new Result<T> { IsSuccess = false, Message = message, Status = status };
    }


    public static Task<Result<T>> FailAsync(string message, HttpStatusCode status)
    {
        return Task.FromResult(Fail(message, status));
    }

    public static Result<T> Success(HttpStatusCode status = default)
    {
        return new Result<T> { IsSuccess = true, Status = status };
    }

    private static Result<T> Success(string message, HttpStatusCode status = default)
    {
        return new Result<T> { IsSuccess = true, Message = message, Status = status };
    }

    private static Result<T> Success(T data, HttpStatusCode status = default)
    {
        return new Result<T> { IsSuccess = true, Data = data, Status = status };
    }

    private static Result<T> Success(T data, string message, HttpStatusCode status)
    {
        return new Result<T> { IsSuccess = true, Data = data, Message = message, Status = status };
    }

    public static Task<Result<T>> SuccessAsync(T data, string message, HttpStatusCode status)
    {
        return Task.FromResult(Success(data, message, status));
    }
}