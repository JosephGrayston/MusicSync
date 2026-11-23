using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MusicSync;
using MusicSync.Infrastructure;
using MusicSync.Infrastructure.Logging;
using MusicSync.Infrastructure.Persistence;
using MusicSync.Playlists;
using MusicSync.Spotify;
using Spectre.Console;


HostApplicationBuilder builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings { Args = args, ContentRootPath = AppContext.BaseDirectory });

var applicationPaths = ApplicationPaths.ForCurrentUser();
applicationPaths.EnsureDirectoriesCreated();

builder.Services
    .AddSingleton(applicationPaths)
    .AddMusicSyncInfrastructure()
    .AddApplicationServices()
    .AddPlaylistServices()
    .AddSpotifyServices()
    .AddMusicSyncPersistence(applicationPaths)
    .AddMusicSyncLogging(builder.Configuration, applicationPaths);

using var host = builder.Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();
var console = host.Services.GetRequiredService<IAnsiConsole>();

using var cts = new CancellationTokenSource();

Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

try
{
    await using var scope = host.Services.CreateAsyncScope();

    var db = scope.ServiceProvider.GetRequiredService<MusicSyncDbContext>();
    await db.Database.MigrateAsync(cts.Token);

    var app = scope.ServiceProvider.GetRequiredService<Application>();
    await app.RunAsync(cts.Token);
    
    return ExitCodes.Success;
}
catch (OperationCanceledException) when (cts.IsCancellationRequested)
{
    return ExitCodes.Cancelled;
}
catch (Exception ex)
{
    MusicSyncLog.CriticalApplicationError(logger, ex);

    console.MarkupLine($"[red]Something went wrong. Please check the log file for details: {applicationPaths.LogDirectory}[/]");

    return ExitCodes.Error;
}
