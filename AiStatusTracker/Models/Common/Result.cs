namespace AiStatusTracker.Models.Common;

public class Result<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public ErrorResponse? Error { get; init; }

    public static Result<T> Ok(T data) => new() { Success = true, Data = data };

    public static Result<T> Fail(string code, string message) =>
        new() { Success = false, Error = new ErrorResponse(code, message) };
}

public record ErrorResponse(string Code, string Message);