using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicSync.Infrastructure;
using MusicSync.Persistence;
using MusicSync.Playlists;
using MusicSync.Spotify;
using Serilog;
using System.Globalization;

namespace MusicSync.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<Application>();
        services.AddScoped<SyncPlaylistService>();
        services.AddScoped<PlaylistSelector>();
        services.AddScoped<SpotifyPlaylistImporter>();
        services.AddScoped<ISpotifyLauncher, SpotifyLauncher>();

        return services;
    }

    public static IServiceCollection ConfigureEntityFramework(this IServiceCollection services, ApplicationPaths applicationPaths) => services.AddDbContext<MusicSyncDbContext>(x => x.UseSqlite($"Data Source={applicationPaths.DatabaseFile}"));

    public static IServiceCollection ConfigureLogging(this IServiceCollection services, IConfiguration configuration, ApplicationPaths applicationPaths)
    {
        // TO DO: check log file name?
        services.AddSerilog((services, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(configuration)
                .ReadFrom.Services(services)
                .WriteTo.File(
                    Path.Combine(applicationPaths.LogDirectory, "musicsync-.log"),
                    formatProvider: CultureInfo.InvariantCulture,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7);
        });

        return services;
    }
}
