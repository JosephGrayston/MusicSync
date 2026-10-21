using MusicSync.Playlists;
using MusicSync.Spotify;

namespace MusicSync;

public sealed class SyncPlaylistService(
    PlaylistSelector playlistSelector,
    SpotifyPlaylistImporter spotifyPlaylistImporter)
{
    public async Task HandleAsync(CancellationToken cancellationToken)
    {
        var playlistId = await playlistSelector.SelectAsync(cancellationToken);

        await spotifyPlaylistImporter.ImportAsync(playlistId);
    }
}
