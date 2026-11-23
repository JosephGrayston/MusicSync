using MusicSync.Infrastructure.Terminal;

namespace MusicSync;

public sealed class Application(ConsoleBanner consoleBanner, SyncPlaylistService syncPlaylistService)
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        consoleBanner.DisplayBanner();

        await syncPlaylistService.ExecuteAsync(cancellationToken);
    }
}
