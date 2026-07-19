using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Додаємо EF Core
using Shared.Extensions;


namespace Fenixa.Api.Infrastructure;

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
        if (exception is ValidationException validationException)
        {
            var errors = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray());

            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            await httpContext.Response.WriteAsJsonAsync(new ValidationProblemDetails(errors)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation failed."
            }, cancellationToken);

            return true;
        }

        if (exception is DbUpdateException dbEx &&
                    dbEx.TryGetUniqueConstraintViolation(out var rawConstraintName))
        {
            var cleanFieldName = DbUpdateExceptionExtensions.CleanConstraintName(rawConstraintName);

            _logger.LogWarning(exception, "Unique constraint violation on constraint: {ConstraintName}", rawConstraintName);

            httpContext.Response.StatusCode = StatusCodes.Status409Conflict;

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Resource conflict.",
                Detail = $"A record with this '{cleanFieldName}' already exists."
            };

            problemDetails.Extensions["violatedField"] = cleanFieldName;
            //problemDetails.Extensions["rawConstraint"] = rawConstraintName;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }

        _logger.LogError(exception, "Unhandled exception occurred.");

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An unexpected error occurred."
        }, cancellationToken);

        return true;
    }
}