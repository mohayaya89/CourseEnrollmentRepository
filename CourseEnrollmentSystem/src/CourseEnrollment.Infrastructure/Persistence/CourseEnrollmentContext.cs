using CourseEnrollment.Domain.Entities;
using CourseEnrollment.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CourseEnrollment.Infrastructure.Persistence
{
    /// <summary>
    /// Main EF Core database context. Inherits <see cref="IdentityDbContext{TUser,TRole,TKey}"/>
    /// so Identity tables are owned by the same schema as domain tables.
    /// </summary>
    public class CourseEnrollmentContext
        : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        public CourseEnrollmentContext(DbContextOptions<CourseEnrollmentContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CourseEnrollmentContext).Assembly);
        }
    }
}
