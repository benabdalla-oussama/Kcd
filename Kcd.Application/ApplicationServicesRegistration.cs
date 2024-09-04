using Kcd.Application.Interfaces;
using Kcd.Application.Services;
using Kcd.Identity.Services;
using Kcd.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Kcd.Application;

public static class ApplicationServicesRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register Application Services
        services.AddScoped<IApplicationService, ApplicationService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IFileValidator, FileValidator>();
        services.AddScoped<IAvatarService, AvatarService>();

        // Register AutoMapper
        services.AddAutoMapper(typeof(ApplicationServicesRegistration).Assembly);

        return services;
    }
}
