using Shared.OperationResults;
using Microsoft.AspNetCore.Http;

namespace Shared.Extensions;

public static class ResultHttpExtensions
{
    public static IResult ToHttpResult(this Result result)
    {
        if (result.IsSuccess)
        {
            return Results.Ok();
        }

        return MapError(result.Error);
    }

    public static IResult ToHttpResult(this Result result, Func<IResult> onSuccess)
    {
        if (result.IsSuccess)
        {
            return onSuccess();
        }

        return MapError(result.Error);
    }

    public static IResult ToHttpResult<T>(this Result<T> result, Func<T, IResult> onSuccess)
    {
        if (result.IsSuccess)
        {
            return onSuccess(result.Value);
        }

        return MapError(result.Error);
    }

    private static IResult MapError(Error error) => error.Type switch
    {
        ErrorType.NotFound => Results.NotFound(new { error.Code, error.Message }),
        ErrorType.Conflict => Results.Conflict(new { error.Code, error.Message }),
        ErrorType.Unauthorized => Results.Unauthorized(),
        ErrorType.Forbidden => Results.Json(
            new { error.Code, error.Message },
            statusCode: StatusCodes.Status403Forbidden),
        ErrorType.Validation => Results.BadRequest(new { error.Code, error.Message }),
        _ => Results.Json(
            new { error.Code, error.Message },
            statusCode: StatusCodes.Status400BadRequest)
    };
}
