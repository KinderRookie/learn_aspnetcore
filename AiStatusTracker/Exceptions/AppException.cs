using System.Net;

namespace AiStatusTracker.Exceptions;

public class AppException : Exception
{
    public string Code { get; }
    public int StatusCode { get; }

    public AppException(string code, string message, int statusCode = (int)HttpStatusCode.BadRequest) : base(message)
    {
        Code = code;
        StatusCode = statusCode;
    }
    
}

public class NotFoundException : AppException
{
    public NotFoundException(string message, string code = "NOT_FOUND")
        : base(code, message, (int)HttpStatusCode.NotFound) { }
}

public class UnauthorizedAppException : AppException
{
    public UnauthorizedAppException(string message, string code = "UNAUTHORIZED")
        : base(code, message, (int)HttpStatusCode.Unauthorized) { }
}

public class ForbiddenAppException : AppException
{
    public ForbiddenAppException(string message, string code = "FORBIDDEN")
        : base(code, message, (int)HttpStatusCode.Forbidden) { }
}