using Microsoft.Extensions.Logging;
using MusicSync.Banner;
using Spectre.Console;

namespace MusicSync
{
    public sealed class Application(SyncPlaylistService syncPlaylistHandler)
    {
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            ConsoleBanner.DisplayBanner();

            //TO DO: Cancellation token

            try
            {
                await syncPlaylistHandler.HandleAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                // TO DO: Add logging for the exception
                AnsiConsole.Markup($"\n[red]{Markup.Escape(ex.Message)}[/]");
            }
        }
    }
}
