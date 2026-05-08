using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CourseEnrollment.Application.Common.Auth.Dtos;
using CourseEnrollment.Application.Common.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CourseEnrollment.Infrastructure.Identity
{
    /// <summary>Creates and validates JWT access tokens using HMAC-SHA256 signing.</summary>
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _settings;

        public JwtTokenService(IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
        }

        /// <inheritdoc />
        public (string Token, DateTime ExpiresAt) GenerateAccessToken(
            ApplicationUserDto user, IEnumerable<string> roles)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            if (user.StudentId.HasValue)
                claims.Add(new Claim("student_id", user.StudentId.Value.ToString()));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiresAt = DateTime.UtcNow.AddMinutes(_settings.AccessTokenMinutes);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: creds);

            return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
        }

        /// <inheritdoc />
        public string GenerateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes);
        }

        /// <inheritdoc />
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var validationParams = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _settings.Issuer,
                ValidateAudience = true,
                ValidAudience = _settings.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key)),
                ValidateLifetime = false, // lifetime is intentionally skipped here
                ClockSkew = TimeSpan.Zero,
            };

            var principal = new JwtSecurityTokenHandler()
                .ValidateToken(token, validationParams, out var securityToken);

            if (securityToken is not JwtSecurityToken jwt ||
                !jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
                throw new SecurityTokenException("Invalid token.");

            return principal;
        }
    }
}
