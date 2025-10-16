using System.Net;
using System.Text.Json;
using AiStatusTracker.Exceptions;
using AiStatusTracker.Models.Common;

namespace AiStatusTracker.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, _logger);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex,
        ILogger logger)
    {
        var status = (int)HttpStatusCode.InternalServerError;
        var code = "UNHANDLED_EXCEPTION";
        var message = "서버에서 예기치 못한 오류가 발생했습니다.";
        
        // 추적용 traceId (로그-응답 연결)
        var traceId = context.TraceIdentifier;

        switch (ex)
        {
            case AppException appEx:
                status = appEx.StatusCode;
                code = appEx.Code;
                message = appEx.Message;
                break;

            case OperationCanceledException:
                status = 499;
                code = "REQUEST_CANCELLED";
                message = "클라이언트가 요청을 취소했습니다.";
                break;

            case UnauthorizedAccessException:
                status = (int)HttpStatusCode.Unauthorized;
                code = "UNAUTHORIZED";
                message = "인증이 필요합니다.";
                break;

            case KeyNotFoundException:
                status = (int)HttpStatusCode.NotFound;
                code = "NOT_FOUND";
                message = ex.Message;
                break;
        }
        
        
        // 구조적 로그
        if (status >= 500)
            logger.LogError(ex, "Unhandled exception. TraceId={TraceId}, Code={Code}", traceId, code);
        else
            logger.LogWarning(ex, "Handled app exception. TraceId={TraceId}, Code={Code}", traceId, code);

        if (!context.Response.HasStarted)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = status;

            // 표준 Result 래핑 (data는 null)
            var result = Result<object>.Fail(code, message);

            // traceId 같은 메타정보를 에러 메시지에 포함하고 싶다면 여기서 확장 가능
            // 예: result.Error = new ErrorResponse(code, $"{message} (traceId: {traceId})");

            await context.Response.WriteAsync(JsonSerializer.Serialize(result, JsonOptions));
        }
    }
}