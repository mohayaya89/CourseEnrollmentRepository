using CourseEnrollment.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseEnrollment.Infrastructure.Persistence.Configurations
{
    /// <summary>EF Core configuration for <see cref="RefreshToken"/>.</summary>
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.TokenHash)
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(t => t.ReplacedByTokenHash)
                .HasMaxLength(64);

            builder.HasIndex(t => t.UserId);
            builder.HasIndex(t => t.TokenHash).IsUnique();
        }
    }
}
