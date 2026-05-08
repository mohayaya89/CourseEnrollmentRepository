using System;
using System.Collections.Generic;
using System.Text;
using CourseEnrollment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseEnrollment.Infrastructure.Persistence.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(c => c.Status)
                   .IsRequired();

            // Course extends BasePerson for Id/DomainEvents but these person-specific columns
            // are not meaningful for a course, so we exclude them from the schema.
            builder.Ignore(c => c.Name);
            builder.Ignore(c => c.Email);
            builder.Ignore(c => c.PhoneNumber);
            builder.Ignore(c => c.DateOfBirth);

            builder.HasMany(c => c.Enrollments)
                   .WithOne()
                   .HasForeignKey(e => e.CourseId);
        }
    }

}
