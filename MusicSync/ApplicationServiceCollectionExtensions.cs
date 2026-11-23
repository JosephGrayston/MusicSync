using Microsoft.Extensions.DependencyInjection;

namespace MusicSync;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<Application>();
        services.AddScoped<SyncPlaylistService>();

        return services;
    }
}
