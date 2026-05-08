using CourseEnrollment.Application.Common.Auth;
using CourseEnrollment.Application.Common.Auth.Dtos;
using CourseEnrollment.Application.Common.Interfaces;
using MediatR;

namespace CourseEnrollment.Application.Features.Auth.Commands.Login
{
    /// <summary>
    /// Validates credentials and issues a new access + refresh token pair.
    /// Always returns a generic error message on failure — never reveals which field was wrong.
    /// </summary>
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponseDto>>
    {
        private static readonly string[] InvalidCredentialsError = ["Invalid credentials."];

        private readonly IIdentityService _identity;
        private readonly IJwtTokenService _jwt;
        private readonly IRefreshTokenStore _tokenStore;

        public LoginCommandHandler(
            IIdentityService identity,
            IJwtTokenService jwt,
            IRefreshTokenStore tokenStore)
        {
            _identity = identity;
            _jwt = jwt;
            _tokenStore = tokenStore;
        }

        public async Task<Result<AuthResponseDto>> Handle(LoginCommand request, CancellationToken ct)
        {
            var validateResult = await _identity.ValidatePasswordAsync(request.Email, request.Password);
            if (!validateResult.Succeeded)
                return Result<AuthResponseDto>.Failure(InvalidCredentialsError);

            var userId = validateResult.Value;
            var userResult = await _identity.GetUserByIdAsync(userId);
            if (!userResult.Succeeded || userResult.Value is null)
                return Result<AuthResponseDto>.Failure(InvalidCredentialsError);

            var user = userResult.Value;
            var roles = await _identity.GetRolesAsync(userId);
            var (accessToken, expiresAt) = _jwt.GenerateAccessToken(user, roles);
            var rawRefresh = _jwt.GenerateRefreshToken();
            await _tokenStore.StoreAsync(userId, rawRefresh, DateTime.UtcNow.AddDays(7), ct);

            return Result<AuthResponseDto>.Success(new AuthResponseDto(
                accessToken, expiresAt, userId, user.Email, [.. roles], user.StudentId));
        }
    }
}
