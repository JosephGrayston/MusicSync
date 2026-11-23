using Microsoft.Extensions.DependencyInjection;

namespace MusicSync.Playlists;

public static class PlaylistServiceCollectionExtensions
{
    public static IServiceCollection AddPlaylistServices(this IServiceCollection services)
    {
        services.AddScoped<PlaylistSelector>();
        services.AddScoped<IPlaylistStore, PlaylistStore>();
        services.AddScoped<IPlaylistPrompts, PlaylistPrompts>();

        return services;
    }
}
