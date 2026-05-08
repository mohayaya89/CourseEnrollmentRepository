using System.Security.Cryptography;
using System.Text;
using CourseEnrollment.Application.Common.Interfaces;
using CourseEnrollment.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CourseEnrollment.Infrastructure.Identity
{
    /// <summary>
    /// Stores and looks up refresh tokens using SHA-256 hashes.
    /// Raw tokens are NEVER persisted; only the hex digest is stored.
    /// </summary>
    public class RefreshTokenStore : IRefreshTokenStore
    {
        private readonly CourseEnrollmentContext _db;

        public RefreshTokenStore(CourseEnrollmentContext db)
        {
            _db = db;
        }

        /// <inheritdoc />
        public async Task StoreAsync(
            Guid userId, string rawToken, DateTime expiresAt, CancellationToken ct = default)
        {
            _db.RefreshTokens.Add(new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                TokenHash = Hash(rawToken),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = expiresAt,
            });
            await _db.SaveChangesAsync(ct);
        }

        /// <inheritdoc />
        public async Task<(Guid TokenId, Guid UserId)?> FindAsync(
            string rawToken, CancellationToken ct = default)
        {
            var hash = Hash(rawToken);
            var token = await _db.RefreshTokens
                .AsNoTracking()
                .Where(t => t.TokenHash == hash)
                .FirstOrDefaultAsync(ct);

            if (token is null || !token.IsActive)
                return null;

            return (token.Id, token.UserId);
        }

        /// <inheritdoc />
        public async Task RevokeAsync(
            Guid tokenId, string? replacedByTokenHash = null, CancellationToken ct = default)
        {
            var token = await _db.RefreshTokens.FindAsync([tokenId], ct);
            if (token is null) return;

            token.RevokedAt = DateTime.UtcNow;
            token.ReplacedByTokenHash = replacedByTokenHash;
            await _db.SaveChangesAsync(ct);
        }

        /// <inheritdoc />
        public async Task RevokeAllForUserAsync(Guid userId, CancellationToken ct = default)
        {
            var tokens = await _db.RefreshTokens
                .Where(t => t.UserId == userId && t.RevokedAt == null)
                .ToListAsync(ct);

            foreach (var t in tokens)
                t.RevokedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(ct);
        }

        private static string Hash(string rawToken)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawToken));
            return Convert.ToHexStringLower(bytes);
        }
    }
}
