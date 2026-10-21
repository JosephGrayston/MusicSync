using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MusicSync;
using MusicSync.Extensions;
using MusicSync.Infrastructure;
using MusicSync.Logging;
using MusicSync.Persistence;


HostApplicationBuilder builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings { Args = args, ContentRootPath = AppContext.BaseDirectory });

var applicationPaths = ApplicationPaths.ForCurrentUser();
applicationPaths.EnsureDirectoriesCreated();

builder.Services
    .AddSingleton(applicationPaths)
    .ConfigureServices()
    .ConfigureEntityFramework(applicationPaths)
    .ConfigureLogging(builder.Configuration, applicationPaths);

using var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MusicSyncDbContext>();
    await db.Database.MigrateAsync(host.Services.GetRequiredService<IHostApplicationLifetime>().ApplicationStopping);
}

var logger = host.Services.GetRequiredService<ILogger<Program>>();

using var cts = new CancellationTokenSource();

Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

try
{
    await using var scope = host.Services.CreateAsyncScope();
    var app = scope.ServiceProvider.GetRequiredService<Application>();

    await app.RunAsync(cts.Token);
    return 0;
}
catch (OperationCanceledException)
{
    // TO DO: Avoid magic numbers (Possibly enum), find source for standard exit codes
    return 130;
}
catch (Exception ex)
{
    MusicSyncLog.CriticalApplicationError(logger, ex);
    Environment.ExitCode = 1;
}

return 1;