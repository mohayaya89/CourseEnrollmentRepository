using System;
using System.Collections.Generic;
using System.Text;
using CourseEnrollment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseEnrollment.Infrastructure.Persistence.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id);

            builder.OwnsOne(s => s.Name, name =>
            {
                name.Property(n => n.FistName).HasColumnName("FirstName").IsRequired();
                name.Property(n => n.LastName).HasColumnName("LastName").IsRequired();
            });

            builder.OwnsOne(s => s.Email, email =>
            {
                email.Property(e => e.Value).HasColumnName("Email").IsRequired();
            });
        }
    }
}
