using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicSync.Infrastructure.Console;
using MusicSync.Infrastructure.Persistence;
using MusicSync.Playlists;
using MusicSync.Spotify;
using Serilog;
using Spectre.Console;
using System.Globalization;

namespace MusicSync.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMusicSyncServices(this IServiceCollection services)
    {
        services.AddScoped<Application>();
        services.AddScoped<SyncPlaylistService>();
        services.AddScoped<PlaylistSelector>();
        services.AddScoped<SpotifyPlaylistImporter>();
        services.AddScoped<ISpotifyLauncher, SpotifyLauncher>();
        services.AddScoped<IPlaylistStore, PlaylistStore>();
        services.AddScoped<IPlaylistPrompts, PlaylistPrompts>();
        services.AddSingleton<ConsoleBanner>();
        services.AddSingleton<DateTimeFormatter>();

        services.AddSingleton(TimeProvider.System);
        services.AddSingleton(AnsiConsole.Console);

        return services;
    }

    public static IServiceCollection AddPersistance(this IServiceCollection services, ApplicationPaths applicationPaths) => services.AddDbContext<MusicSyncDbContext>(x => x.UseSqlite(new SqliteConnectionStringBuilder() { DataSource = applicationPaths.DatabaseFile }.ToString()));

    public static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration configuration, ApplicationPaths applicationPaths)
    {
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
