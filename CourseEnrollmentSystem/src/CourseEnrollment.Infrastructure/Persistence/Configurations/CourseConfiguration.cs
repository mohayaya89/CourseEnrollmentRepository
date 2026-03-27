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

            // Use HasMany if Enrollment is an entity with Id and FK
            builder.HasMany(c => c.Enrollments)
                   .WithOne() // or .WithOne(e => e.Course) if navigation exists
                   .HasForeignKey(e => e.CourseId);
        }
    }

}
