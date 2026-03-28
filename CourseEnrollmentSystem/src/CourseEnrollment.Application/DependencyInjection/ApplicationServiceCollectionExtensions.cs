using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace CourseEnrollment.Application
{
    public static class ApplicationServiceCollectionExtensions
    {
        // Adds application-layer services. Expand registrations as needed.
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(
                cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly)
                );

            services.AddValidatorsFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);

            return services;
        }
    }
}
