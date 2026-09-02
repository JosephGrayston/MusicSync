using Microsoft.Extensions.DependencyInjection;
using MusicSync.Commands;

namespace MusicSync.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<Application>();

            services.AddScoped<IPlaylistCommand, FetchDestinationPlaylistCommand>();
            services.AddScoped<IPlaylistCommand, ComparePlaylistCommand>();
            services.AddScoped<IPlaylistCommand, PublishPlaylistCommand>();
        }
    }
}
