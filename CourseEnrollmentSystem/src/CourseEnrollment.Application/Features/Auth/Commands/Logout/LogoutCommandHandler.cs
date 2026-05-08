using CourseEnrollment.Application.Common.Interfaces;
using MediatR;

namespace CourseEnrollment.Application.Features.Auth.Commands.Logout
{
    /// <summary>
    /// Revokes ALL active refresh tokens for the user so no device can silently re-authenticate.
    /// The controller is responsible for clearing the HttpOnly cookie after this handler returns.
    /// </summary>
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
    {
        private readonly IRefreshTokenStore _tokenStore;

        public LogoutCommandHandler(IRefreshTokenStore tokenStore)
        {
            _tokenStore = tokenStore;
        }

        public async Task Handle(LogoutCommand request, CancellationToken ct)
        {
            await _tokenStore.RevokeAllForUserAsync(request.UserId, ct);
        }
    }
}
