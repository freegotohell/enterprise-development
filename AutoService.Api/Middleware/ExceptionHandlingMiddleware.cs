
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace AutoService.Api.Middleware;

/// <summary>
/// Converts database conflicts and unhandled exceptions to ProblemDetails responses
/// </summary>
public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    /// <summary>
    /// Invokes the next middleware and handles any unhandled exceptions.
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (DbUpdateException exception) when (
            !context.Response.HasStarted &&
            exception.InnerException is MySqlException { Number: 1062 } duplicateKey)
        {
            var title = duplicateKey.Message switch
            {
                string message when message.Contains("IX_cars_license_plate", StringComparison.Ordinal)
                    => "The license plate is already in use.",
                string message when message.Contains("IX_mechanics_passport_number", StringComparison.Ordinal)
                    => "The passport number is already in use.",
                _ => "A record with the same unique value already exists."
            };

            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = title
            }, options: null, contentType: "application/problem+json");
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "An unhandled exception occurred while processing {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            if (context.Response.HasStarted)
            {
                throw;
            }

            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected server error occurred."
            });
        }
    }
}
