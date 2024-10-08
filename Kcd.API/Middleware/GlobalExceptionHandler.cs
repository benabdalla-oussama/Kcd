﻿using Kcd.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Stayr.Backend.Common.Observability;
using System.Net;

namespace Stayr.Backend.Api.Middleware;

/// <summary>
/// Handles exceptions globally across the application, providing a consistent and secure response structure.
/// Logs exceptions and ensures appropriate HTTP status codes and error messages are returned to clients.
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        this._logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken = default)
    {
        _logger.LogError(LogEvents.Api.UnhandledError,
            exception, "Unhandled exception occurred while processing the request: {Message}.", exception.Message);

        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
        ProblemDetails problem = new();

        switch (exception)
        {
            case BadRequestException badRequestException:
                statusCode = HttpStatusCode.BadRequest;
                problem = new ProblemDetails
                {
                    Title = badRequestException.Message,
                    Status = (int)statusCode,
                    Detail = badRequestException.InnerException?.Message,
                    Type = nameof(BadRequestException)
                };
                break;
            case NotFoundException NotFound:
                statusCode = HttpStatusCode.NotFound;
                problem = new ProblemDetails
                {
                    Title = NotFound.Message,
                    Status = (int)statusCode,
                    Type = nameof(NotFoundException),
                    Detail = NotFound.InnerException?.Message,
                };
                break;
            case FileNotFoundException FileNotFound:
                statusCode = HttpStatusCode.NotFound;
                problem = new ProblemDetails
                {
                    Title = FileNotFound.Message,
                    Status = (int)statusCode,
                    Type = nameof(FileNotFoundException),
                    Detail = FileNotFound.InnerException?.Message,
                };
                break;
            default:
                problem = new ProblemDetails
                {
                    Title = exception.Message,
                    Status = (int)statusCode,
                    Type = nameof(HttpStatusCode.InternalServerError),
                    Detail = exception.StackTrace,
                };
                break;
        }

        httpContext.Response.StatusCode = (int)statusCode;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }
}
