using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using MyProject.Application.Common;
using MyProject.Application.Exceptions;

namespace MyProject.WebApi.Middleware;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        _logger.LogError(
            exception,
            "Could not process a request on machine {MachineName}. TraceId: {TraceId}",
            Environment.MachineName,
            traceId
        );

        var (statusCode, message) = MapException(exception);

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";

        var response = Result<string>.Failure(message, statusCode);

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response), cancellationToken);

        return true;
    }

    private static (int statusCode, string message) MapException(Exception exception)
    {
        return exception switch
        {
            NotFoundException ex => (StatusCodes.Status404NotFound, ex.Message),
            Application.Exceptions.ValidationException ex => (StatusCodes.Status400BadRequest, ex.Message),
            ForbiddenAccessException => (StatusCodes.Status403Forbidden, "Access to this resource is forbidden."),
            ArgumentNullException => (StatusCodes.Status400BadRequest, "Invalid request: missing required parameter."),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized access."),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };
    }
}

