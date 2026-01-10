namespace Application.Common;

public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Message { get; }
    public T? Data { get; }
    public int StatusCode { get; }

    private Result(bool isSuccess, T? data, string message, int statusCode)
    {
        IsSuccess = isSuccess;
        Data = data;
        Message = message;
        StatusCode = statusCode;
    }

    public static Result<T> Success(T data, string message = "", int statusCode = 200)
        => new(true, data, message, statusCode);

    public static Result<T> Failure(string message, int statusCode = 400)
        => new(false, default, message, statusCode);
}

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Message { get; }
    public int StatusCode { get; }

    private Result(bool isSuccess, string message, int statusCode)
    {
        IsSuccess = isSuccess;
        Message = message;
        StatusCode = statusCode;
    }

    public static Result Success(string message = "", int statusCode = 200)
        => new(true, message, statusCode);

    public static Result Failure(string message, int statusCode = 400)
        => new(false, message, statusCode);
}

