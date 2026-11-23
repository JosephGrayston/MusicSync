using Microsoft.Extensions.DependencyInjection;

namespace MusicSync.Spotify;

public static class SpotifyServiceCollectionExtensions
{
    public static IServiceCollection AddSpotifyServices(this IServiceCollection services)
    {
        services.AddScoped<SpotifyPlaylistImporter>();
        services.AddScoped<ISpotifyLauncher, SpotifyLauncher>();

        return services;
    }
}
