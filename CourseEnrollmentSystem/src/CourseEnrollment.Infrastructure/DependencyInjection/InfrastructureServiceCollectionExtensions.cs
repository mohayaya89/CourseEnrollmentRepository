using System;
using System.Collections.Generic;
using System.Text;
using CourseEnrollment.Application.Common.Interfaces;
using CourseEnrollment.Domain.Interfaces;
using CourseEnrollment.Domain.Persistence;
using CourseEnrollment.Infrastructure.DomainEvents;
using CourseEnrollment.Infrastructure.Repository;
using CourseEnrollment.Infrastructure.Services;
using CourseEnrollment.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace CourseEnrollment.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Default")));

            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }
    }

}
