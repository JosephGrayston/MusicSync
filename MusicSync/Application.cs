using Microsoft.Extensions.Logging;
using MusicSync.Banner;
using Spectre.Console;

namespace MusicSync
{
    public sealed class Application(SyncPlaylistHandler syncPlaylistHandler, ILogger<Application> logger)
    {
        public async Task RunAsync()
        {
            ConsoleBanner.DisplayBanner();

            //TO DO: Cancellation token

            try
            {
                await syncPlaylistHandler.HandleAsync();
            }
            catch (Exception ex)
            {
                // TO DO: Add logging for the exception
                AnsiConsole.Markup($"\n[red]{Markup.Escape(ex.Message)}[/]");
            }
        }
    }
}
