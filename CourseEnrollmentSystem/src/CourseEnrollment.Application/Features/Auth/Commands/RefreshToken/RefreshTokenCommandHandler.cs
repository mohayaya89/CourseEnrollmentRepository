using CourseEnrollment.Application.Common.Auth;
using CourseEnrollment.Application.Common.Auth.Dtos;
using CourseEnrollment.Application.Common.Interfaces;
using MediatR;

namespace CourseEnrollment.Application.Features.Auth.Commands.RefreshToken
{
    /// <summary>
    /// Validates the supplied raw refresh token against the store, revokes the old token (rotation),
    /// and issues a fresh access + refresh token pair.
    /// </summary>
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<AuthTokenPair>>
    {
        private readonly IRefreshTokenStore _tokenStore;
        private readonly IIdentityService _identity;
        private readonly IJwtTokenService _jwt;

        public RefreshTokenCommandHandler(
            IRefreshTokenStore tokenStore,
            IIdentityService identity,
            IJwtTokenService jwt)
        {
            _tokenStore = tokenStore;
            _identity = identity;
            _jwt = jwt;
        }

        public async Task<Result<AuthTokenPair>> Handle(RefreshTokenCommand request, CancellationToken ct)
        {
            var entry = await _tokenStore.FindAsync(request.RawRefreshToken, ct);
            if (entry is null)
                return Result<AuthTokenPair>.Failure("Refresh token is invalid or expired.");

            var (tokenId, userId) = entry.Value;

            var userResult = await _identity.GetUserByIdAsync(userId);
            if (!userResult.Succeeded || userResult.Value is null)
                return Result<AuthTokenPair>.Failure("User not found.");

            var user = userResult.Value;
            var roles = await _identity.GetRolesAsync(userId);

            var newRawRefresh = _jwt.GenerateRefreshToken();
            var (newAccessToken, expiresAt) = _jwt.GenerateAccessToken(user, roles);

            await _tokenStore.RevokeAsync(tokenId, ct: ct);
            await _tokenStore.StoreAsync(userId, newRawRefresh, DateTime.UtcNow.AddDays(7), ct);

            var dto = new AuthResponseDto(newAccessToken, expiresAt, userId, user.Email, [.. roles], user.StudentId);
            return Result<AuthTokenPair>.Success(new AuthTokenPair(dto, newRawRefresh));
        }
    }
}
