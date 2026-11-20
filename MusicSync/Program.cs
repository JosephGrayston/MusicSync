using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MusicSync;
using MusicSync.Infrastructure;
using MusicSync.Infrastructure.Logging;
using MusicSync.Infrastructure.Persistence;
using Spectre.Console;


HostApplicationBuilder builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings { Args = args, ContentRootPath = AppContext.BaseDirectory });

var applicationPaths = ApplicationPaths.ForCurrentUser();
applicationPaths.EnsureDirectoriesCreated();

builder.Services
    .AddSingleton(applicationPaths)
    .AddMusicSyncServices()
    .AddPersistance(applicationPaths)
    .AddLogging(builder.Configuration, applicationPaths);

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
    using (var serviceScope = host.Services.CreateScope())
    {
        var db = serviceScope.ServiceProvider.GetRequiredService<MusicSyncDbContext>();
        await db.Database.MigrateAsync(host.Services.GetRequiredService<IHostApplicationLifetime>().ApplicationStopping);
    }

    await using var scope = host.Services.CreateAsyncScope();
    var app = scope.ServiceProvider.GetRequiredService<Application>();

    await app.RunAsync(cts.Token);
    return ExitCodes.Success;
}
catch (OperationCanceledException)
{
    return ExitCodes.Cancelled;
}
catch (Exception ex)
{
    MusicSyncLog.CriticalApplicationError(logger, ex);

    console.MarkupLine($"[red]Something went wrong. Please check the log file for details.[/]");

    return ExitCodes.Error;
}
