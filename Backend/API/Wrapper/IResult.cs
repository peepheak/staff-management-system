namespace API.Wrapper;

public interface IResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
}

public interface IResult<out T> : IResult
{
    T? Data { get; }
}