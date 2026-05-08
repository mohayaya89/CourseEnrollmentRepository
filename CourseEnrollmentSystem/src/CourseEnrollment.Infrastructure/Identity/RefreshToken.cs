namespace CourseEnrollment.Infrastructure.Identity
{
    /// <summary>
    /// Persisted refresh token. The raw token is NEVER stored; only its SHA-256 hash.
    /// </summary>
    public class RefreshToken
    {
        /// <summary>Surrogate primary key.</summary>
        public Guid Id { get; set; }

        /// <summary>The owning user.</summary>
        public Guid UserId { get; set; }

        /// <summary>SHA-256 hex hash of the raw token.</summary>
        public string TokenHash { get; set; } = string.Empty;

        /// <summary>When this token was issued.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>When this token expires (UTC).</summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>When this token was revoked. Null means it is still active.</summary>
        public DateTime? RevokedAt { get; set; }

        /// <summary>Hash of the token that replaced this one during rotation.</summary>
        public string? ReplacedByTokenHash { get; set; }

        /// <summary>Whether this token is still usable.</summary>
        public bool IsActive => RevokedAt is null && DateTime.UtcNow < ExpiresAt;
    }
}
