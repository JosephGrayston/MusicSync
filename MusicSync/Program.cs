using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicSync;
using MusicSync.Extensions;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.ConfigureServices();

using var host = builder.Build();

var app = host.Services.GetRequiredService<Application>();

return await app.RunAsync();