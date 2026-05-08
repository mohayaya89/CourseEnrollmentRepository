using CourseEnrollment.Application.Common.Interfaces;
using CourseEnrollment.Domain.Interfaces;
using CourseEnrollment.Infrastructure.DomainEvents;
using CourseEnrollment.Infrastructure.Identity;
using CourseEnrollment.Infrastructure.Persistence;
using CourseEnrollment.Infrastructure.Repository;
using CourseEnrollment.Infrastructure.Services;
using CourseEnrollment.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CourseEnrollment.Infrastructure.DependencyInjection
{
    /// <summary>Extension methods to register Infrastructure services with the DI container.</summary>
    public static class InfrastructureServiceCollectionExtensions
    {
        /// <summary>Registers all Infrastructure services including EF Core, Identity, JWT, and repositories.</summary>
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var conn = configuration.GetConnectionString("CourseEnrollmentContext");
            if (string.IsNullOrWhiteSpace(conn))
                throw new InvalidOperationException(
                    "Connection string 'CourseEnrollmentContext' is not configured.");

            services.AddDbContext<CourseEnrollmentContext>(options =>
                options.UseSqlServer(conn));

            // Identity
            services.AddIdentityCore<ApplicationUser>(options =>
                {
                    options.Password.RequiredLength = 8;
                    options.Password.RequireDigit = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = false;

                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                    options.Lockout.AllowedForNewUsers = true;
                })
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<CourseEnrollmentContext>()
                .AddDefaultTokenProviders();

            // JWT settings — validated eagerly at startup
            services.AddOptions<JwtSettings>()
                .Bind(configuration.GetSection("JwtSettings"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(InfrastructureServiceCollectionExtensions).Assembly));

            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IRefreshTokenStore, RefreshTokenStore>();

            return services;
        }
    }
}
