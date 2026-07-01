using MediatR;

namespace Study.Features.GetStudySession;

public sealed record GetStudySessionQuery(
    Guid UserId,
    string? TargetLanguage,
    int MaxCards) : IRequest<GetStudySessionResponse>;

public sealed record StudyCardDto(
    Guid CardId,
    string FrontText,
    string BackText,
    string ContextExample,
    string AudioUrl,
    string Status);

public sealed record GetStudySessionResponse(IReadOnlyList<StudyCardDto> Cards);
