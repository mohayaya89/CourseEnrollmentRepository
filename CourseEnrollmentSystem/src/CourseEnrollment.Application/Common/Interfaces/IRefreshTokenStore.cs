namespace CourseEnrollment.Application.Common.Interfaces
{
    /// <summary>Persists and looks up hashed refresh tokens.</summary>
    public interface IRefreshTokenStore
    {
        /// <summary>Persists a hashed token for <paramref name="userId"/> with the given expiry.</summary>
        Task StoreAsync(Guid userId, string rawToken, DateTime expiresAt, CancellationToken ct = default);

        /// <summary>
        /// Finds a valid (non-revoked, non-expired) token entry by the raw token value.
        /// Returns <c>null</c> when not found or already revoked.
        /// </summary>
        Task<(Guid TokenId, Guid UserId)?> FindAsync(string rawToken, CancellationToken ct = default);

        /// <summary>Marks a single token as revoked and records the hash of its replacement.</summary>
        Task RevokeAsync(Guid tokenId, string? replacedByTokenHash = null, CancellationToken ct = default);

        /// <summary>Revokes all active refresh tokens for <paramref name="userId"/>.</summary>
        Task RevokeAllForUserAsync(Guid userId, CancellationToken ct = default);
    }
}
