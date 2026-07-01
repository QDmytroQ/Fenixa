using MediatR;

namespace Identity.Features.UpdateGeminiKey;

public sealed record UpdateGeminiKeyCommand(Guid UserId, string ApiKey) : IRequest;
