namespace Shared.OperationResults;

public sealed record Error(string Code, string Message, ErrorType Type = ErrorType.Failure)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public static Error NotFound(string message) =>
        new("NotFound", message, ErrorType.NotFound);

    public static Error Conflict(string message) =>
        new("Conflict", message, ErrorType.Conflict);

    public static Error Forbidden(string message) =>
        new("Forbidden", message, ErrorType.Forbidden);

    public static Error Validation(string message) =>
        new("Validation", message, ErrorType.Validation);

    public static Error Unauthorized(string message) =>
        new("Unauthorized", message, ErrorType.Unauthorized);
}
