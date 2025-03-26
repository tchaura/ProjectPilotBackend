using System.Net;
using System.Text.Json;
using Common;
using Microsoft.AspNetCore.Diagnostics;
using ProjectPilot.Exceptions;

namespace ProjectPilotWeb.Middleware;

public class ApiExceptionHandler(ILogger<ApiExceptionHandler> logger) : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var exceptionMessage = exception.Message;
        logger.LogError("API Error: {ExceptionMessage}, Time: {Time}", exceptionMessage, DateTime.UtcNow);

        var responseMessage = exception.Message;
        int statusCode;
        var isHandled = true;
        
        switch (exception)
        {
            case WebApiException webApiException:
            {
                statusCode = webApiException.StatusCode;
                break;
            }
            case ConfigurationMissingException:
            default:
            {
                statusCode = StatusCodes.Status500InternalServerError;
                responseMessage = "Server error";
                isHandled = false;
                break;
            }
        }
        
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;
        var responseData = new { message = responseMessage };
        httpContext.Response.WriteAsync(JsonSerializer.Serialize(responseData), cancellationToken: cancellationToken);
        
        return new ValueTask<bool>(isHandled);
    }
}