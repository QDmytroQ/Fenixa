using MediatR;

namespace Profile.Features.GetUserSettings;

public sealed class GetUserSettingsHandler : IRequestHandler<GetUserSettingsQuery, GetUserSettingsResponse>
{
    public Task<GetUserSettingsResponse> Handle(GetUserSettingsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
