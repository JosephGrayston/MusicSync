using MusicSync.Infrastructure.Console;

namespace MusicSync;

public sealed class Application(ConsoleBanner consoleBanner, SyncPlaylistService syncPlaylistHandler)
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        consoleBanner.DisplayBanner();

        await syncPlaylistHandler.HandleAsync(cancellationToken);
    }
}
