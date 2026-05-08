using CourseEnrollment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseEnrollment.Infrastructure.Persistence.Configurations
{
    /// <summary>EF Core configuration for <see cref="Enrollment"/>.</summary>
    public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Status).IsRequired();
            builder.Property(e => e.EnrollmentDate).IsRequired();

            // Enrollment extends BasePerson for Id/DomainEvents; person-specific columns
            // are not meaningful here and are excluded from the schema.
            builder.Ignore(e => e.Name);
            builder.Ignore(e => e.Email);
            builder.Ignore(e => e.PhoneNumber);
            builder.Ignore(e => e.DateOfBirth);
        }
    }
}
