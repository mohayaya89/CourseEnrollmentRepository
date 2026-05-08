using CourseEnrollment.Application.Common.Auth;
using CourseEnrollment.Application.Common.Auth.Dtos;
using MediatR;

namespace CourseEnrollment.Application.Features.Auth.Commands.RefreshToken
{
    /// <summary>
    /// Rotates the refresh token: validates the supplied raw token, revokes it,
    /// and issues a new access + refresh token pair.
    /// </summary>
    public record RefreshTokenCommand(string RawRefreshToken) : IRequest<Result<AuthTokenPair>>;
}
