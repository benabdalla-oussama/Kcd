using Kcd.Application.Interfaces;
using Kcd.Application.Services;
using Kcd.Identity.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Kcd.Identity
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            // Register Application Services
            services.AddScoped<IUserApplicationService, UserApplicationService>();
            services.AddScoped<IAuthService, AuthService>();

            // Register AutoMapper
            services.AddAutoMapper(typeof(ApplicationServicesRegistration).Assembly);

            return services;
        }
    }
}
