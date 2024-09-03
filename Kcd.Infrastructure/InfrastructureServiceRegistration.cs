using Kcd.Infrastructure.Models;
using Kcd.Infrastructure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kcd.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<AvatarSettings>(options =>
            configuration.GetSection(AvatarSettings.SectionKey).Bind(options));

        services.AddSingleton<IAvatarStorageStrategy, FileSystemAvatarStorageStrategy>();
        services.AddSingleton<IAvatarStorageStrategy, BlobAvatarStorageStrategy>();
        services.AddSingleton<IAvatarStorageStrategy, DatabaseAvatarStorageStrategy>();

        return services;
    }
}
