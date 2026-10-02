using MusicSync.Playlists;
using MusicSync.Spotify;

namespace MusicSync;

public sealed class SyncPlaylistHandler(
    PlaylistSelector playlistSelector,
    SpotifyPlaylistImporter spotifyPlaylistImporter)
{
    public async Task HandleAsync()
    {
        var playlistId = await playlistSelector.SelectAsync();

        await spotifyPlaylistImporter.ImportAsync(playlistId);
    }
}
