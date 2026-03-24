using System;
using System.Collections.Generic;
using System.Text;
using CourseEnrollment.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseEnrollment.Domain.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Student> Students => Set<Student>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
