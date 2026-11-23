namespace MusicSync.Playlists;

public sealed class PlaylistSelector(IPlaylistPrompts prompts, IPlaylistStore store)
{
    public async Task<int> SelectAsync(CancellationToken cancellationToken)
    {
        var playlists = await store.GetAllAsync(cancellationToken);

        var action = playlists.Count == 0
            ? PlaylistAction.CreateNewPlaylist
            : await prompts.AskForPlaylistActionAsync(cancellationToken);

        var playlist = action switch
        {
            PlaylistAction.SelectExistingPlaylist => await prompts.ChoosePlaylistAsync(playlists, cancellationToken),
            PlaylistAction.CreateNewPlaylist => await CreateAsync(playlists, cancellationToken),
            _ => throw new InvalidOperationException($"Unknown playlist action: {action}")
        };

        return playlist.Id;
    }

    private async Task<Playlist> CreateAsync(IReadOnlyList<Playlist> existingPlaylists, CancellationToken cancellationToken)
    {
        var existingNames = existingPlaylists
            .Select(p => p.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);


        var name = await prompts.AskForNewPlaylistNameAsync(existingNames, cancellationToken);
        return await store.CreateAsync(name, cancellationToken);
    }
}
