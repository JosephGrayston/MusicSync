using MusicSync.Infrastructure.Console;
using Spectre.Console;

namespace MusicSync.Playlists;

public sealed class PlaylistPrompts(IAnsiConsole console, DateTimeFormatter formatter) : IPlaylistPrompts
{
    // TO DO : Add converter for PlaylistAction to display user-friendly names in the prompt
    public async Task<PlaylistAction> AskForPlaylistActionAsync(CancellationToken cancellationToken)
    {
        return await console.PromptAsync(new SelectionPrompt<PlaylistAction>()
            .Title("\nPlease select an option:")
            .UseConverter(action => action switch
            {
                PlaylistAction.SelectExistingPlaylist => "Select an existing playlist",
                PlaylistAction.CreateNewPlaylist => "Create a new playlist",
                _ => throw new ArgumentOutOfRangeException(nameof(action))
            })
            .AddChoices(PlaylistAction.SelectExistingPlaylist, PlaylistAction.CreateNewPlaylist), cancellationToken);
    }

    public async Task<Playlist> ChoosePlaylistAsync(IReadOnlyList<Playlist> playlists, CancellationToken cancellationToken)
    {
        // Add DateTime Formatter
        return await console.PromptAsync(new SelectionPrompt<Playlist>()
            .Title("\nSearch for a playlist:")
            .PageSize(10)
            .EnableSearch()
            .AddChoices(playlists)
            .UseConverter(p => Markup.Escape($"{p.Name} (Created: {formatter.ToDisplay(p.CreatedAt)})")), cancellationToken);
    }

    public async Task<string> AskForNewPlaylistNameAsync(CancellationToken cancellationToken)
    {
        return await console.AskAsync<string>("Enter the name of the new playlist:", cancellationToken);
    }
}
