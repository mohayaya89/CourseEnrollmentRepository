using System.ComponentModel.DataAnnotations;

namespace CourseEnrollment.Infrastructure.Identity
{
    /// <summary>Configuration values for JWT token issuance, bound from <c>JwtSettings</c> in appsettings.</summary>
    public class JwtSettings
    {
        [Required]
        public string Issuer { get; set; } = string.Empty;

        [Required]
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// HMAC-SHA256 signing key (base-64 encoded, at least 32 bytes decoded).
        /// Must be supplied via user-secrets or a secret manager — never committed to source.
        /// <!-- TODO: migrate to RS256 (asymmetric) before production: eliminates shared-secret
        ///      distribution risk and allows public-key verification by downstream services. -->
        /// </summary>
        [Required, MinLength(1)]
        public string Key { get; set; } = string.Empty;

        /// <summary>Access token lifetime in minutes. Default: 15.</summary>
        public int AccessTokenMinutes { get; set; } = 15;

        /// <summary>Refresh token lifetime in days. Default: 7.</summary>
        public int RefreshTokenDays { get; set; } = 7;
    }
}
