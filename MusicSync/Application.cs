using MusicSync.Infrastructure.Console;
using Spectre.Console;

namespace MusicSync;

public sealed class Application(ConsoleBanner consoleBanner, IAnsiConsole console, SyncPlaylistService syncPlaylistHandler)
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        consoleBanner.DisplayBanner();

        //TO DO: Cancellation token

        try
        {
            await syncPlaylistHandler.HandleAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            // TO DO: Add logging for the exception
            console.Markup($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }
    }
}
