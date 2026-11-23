using MusicSync.Infrastructure.Terminal;

namespace MusicSync;

public sealed class Application(ConsoleBanner consoleBanner, SyncPlaylistService sycnPlaylistService)
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        consoleBanner.DisplayBanner();

        await sycnPlaylistService.ExecuteAsync(cancellationToken);
    }
}
