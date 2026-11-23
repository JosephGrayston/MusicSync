namespace MusicSync.Playlists;

public interface IPlaylistPrompts
{
    Task<PlaylistAction> AskForPlaylistActionAsync(CancellationToken cancellationToken);

    Task<Playlist> ChoosePlaylistAsync(IReadOnlyList<Playlist> playlists, CancellationToken cancellationToken);

    Task<string> AskForNewPlaylistNameAsync(IEnumerable<string> existingPlaylistNames, CancellationToken cancellationToken);
}
