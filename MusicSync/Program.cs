using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MusicSync;
using MusicSync.Extensions;
using MusicSync.Logging;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services
    .ConfigureServices()
    .ConfigureEntityFramework()
    .ConfigureLogging(builder.Configuration);

using var host = builder.Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();

try
{
    var app = host.Services.GetRequiredService<Application>();

    await app.RunAsync();
}
catch (Exception ex)
{
    MusicSyncLog.CritialApplicationError(logger, ex);
    Environment.ExitCode = 1;
}