namespace MusicSync.Playlists;

public sealed partial class PlaylistSelector(IPlaylistPrompts prompts, IPlaylistStore store)
{
    // TO DO: Revist this as the logic is awful
    public async Task<int> SelectAsync(CancellationToken cancellationToken)
    {
       var playlists = await store.GetAllAsync(cancellationToken);

        Playlist playlist;

        if (playlists.Count == 0)
        {
            var playlistName = await prompts.AskForNewPlaylistNameAsync(cancellationToken);
            playlist = await store.CreateAsync(playlistName, cancellationToken);
        }
        else
        {
            var action = await prompts.AskForPlaylistActionAsync(cancellationToken);
            playlist = action switch
            {
                PlaylistAction.SelectExistingPlaylist => await prompts.ChoosePlaylistAsync(playlists, cancellationToken),
                PlaylistAction.CreateNewPlaylist => await store.CreateAsync(await prompts.AskForNewPlaylistNameAsync(cancellationToken), cancellationToken),
                _ => throw new InvalidOperationException($"Unknown playlist action: {action}")
            };

        }

        return playlist.Id;   
    }
}
