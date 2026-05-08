using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CourseEnrollment.Infrastructure.Persistence
{
    /// <summary>
    /// Provides a <see cref="CourseEnrollmentContext"/> for EF Core design-time tools
    /// (migrations, scaffolding) without requiring a running host.
    /// Uses the API project's appsettings.json to resolve the connection string.
    /// </summary>
    internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CourseEnrollmentContext>
    {
        public CourseEnrollmentContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(),
                    "..", "CourseEnrollment.Api"))
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

            var conn = configuration.GetConnectionString("CourseEnrollmentContext")
                ?? throw new InvalidOperationException(
                    "Connection string 'CourseEnrollmentContext' not found for design-time factory.");

            var optionsBuilder = new DbContextOptionsBuilder<CourseEnrollmentContext>();
            optionsBuilder.UseSqlServer(conn);
            return new CourseEnrollmentContext(optionsBuilder.Options);
        }
    }
}
