using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using FluentValidation;
using Blocks.Exceptions;
using Microsoft.Extensions.Logging;

namespace Blocks.AspNetCore;

public sealed class GlobalExceptionMiddleware(RequestDelegate _next, ILogger<GlobalExceptionMiddleware> _logger)
{
    private static HttpStatusCode MapStatusCode(Exception ex) => ex switch
    {
        ValidationException => HttpStatusCode.BadRequest,
        ArgumentException => HttpStatusCode.BadRequest,
        BadRequestException => HttpStatusCode.BadRequest,
        NotFoundException => HttpStatusCode.NotFound,
        DomainException => HttpStatusCode.BadRequest,
        _ => HttpStatusCode.InternalServerError
    };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await HandleValidationExceptionAsync(context, ex, HttpStatusCode.BadRequest);
        }
        catch (OperationCanceledException)
        {
            if (!context.Response.HasStarted) context.Response.StatusCode = 499;
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var statusCode = MapStatusCode(ex);
        if (statusCode == HttpStatusCode.InternalServerError)
            _logger.LogError(ex, "Unhandled exception. TraceId={TraceId}", context.TraceIdentifier);
        else
            _logger.LogInformation(ex, ex.Message, context.TraceIdentifier);

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            context.Response.StatusCode,
            ex.Message,
            Details = ex.StackTrace
        };
        var responseJson = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(responseJson);
    }

    private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception, HttpStatusCode statusCode)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";
        var validationErrors = exception.Errors.Select(e => new
        {
            e.PropertyName,
            e.ErrorMessage
        });

        var response = new
        {
            context.Response.StatusCode,
            exception.Message,
            Details = exception.StackTrace,
            Errors = validationErrors
        };
        var responseJson = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(responseJson);
    }
}
