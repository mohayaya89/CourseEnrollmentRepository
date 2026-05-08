using System.Security.Claims;
using CourseEnrollment.Application.Common.Auth.Dtos;

namespace CourseEnrollment.Application.Common.Interfaces
{
    /// <summary>Creates and validates JWT access tokens.</summary>
    public interface IJwtTokenService
    {
        /// <summary>Generates a signed JWT access token for the given user and roles.</summary>
        /// <returns>The compact serialized token string and its expiry.</returns>
        (string Token, DateTime ExpiresAt) GenerateAccessToken(ApplicationUserDto user, IEnumerable<string> roles);

        /// <summary>Generates a cryptographically random raw refresh token string.</summary>
        string GenerateRefreshToken();

        /// <summary>
        /// Extracts the <see cref="ClaimsPrincipal"/> from an expired (or valid) access token
        /// without validating lifetime. Used during the refresh flow.
        /// </summary>
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
