using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicSync.Data;
using MusicSync.Playlists;
using MusicSync.Spotify;
using Serilog;
using System.Globalization;

namespace MusicSync.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddSingleton<Application>();
        services.AddSingleton<SyncPlaylistHandler>();
        services.AddSingleton<PlaylistSelector>();
        services.AddSingleton<SpotifyPlaylistImporter>();
        services.AddSingleton<ISpotifyLauncher, SpotifyLauncher>();

        return services;
    }

    public static IServiceCollection ConfigureEntityFramework(this IServiceCollection services) => services.AddDbContext<MusicSyncDbContext>();

    public static IServiceCollection ConfigureLogging(this IServiceCollection services, IConfiguration configuration)
    {
        var appDataDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "MusicSync");

        var logDirectory = Path.Combine(appDataDirectory, "logs");

        Directory.CreateDirectory(logDirectory);

        services.AddSerilog((services, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(configuration)
                .ReadFrom.Services(services)
                .WriteTo.File(
                    Path.Combine(logDirectory, "musicsync-.log"),
                    formatProvider: CultureInfo.InvariantCulture,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7);
        });

        return services;
    }
}
