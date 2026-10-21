using Microsoft.EntityFrameworkCore;
using MusicSync.Persistence;
using Spectre.Console;

namespace MusicSync.Playlists;

public sealed class PlaylistSelector(MusicSyncDbContext dbContext)
{
    private enum PlaylistOption
    {
        SelectExistingPlaylist,

        CreateNewPlaylist
    }

    public async Task<int> SelectAsync(CancellationToken cancellationToken)
    {
        // TO DO: inject prompt class here
        // Add converter for PlaylistOption to display user-friendly names in the prompt
        var option = await AnsiConsole.PromptAsync(new SelectionPrompt<PlaylistOption>()
            .Title("\nPlease select an option:")
            .AddChoices(PlaylistOption.SelectExistingPlaylist, PlaylistOption.CreateNewPlaylist), cancellationToken);

        return option switch
        {
            PlaylistOption.SelectExistingPlaylist => await SelectExistingPlaylistAsync(cancellationToken),
            PlaylistOption.CreateNewPlaylist => await CreateNewPlaylistAsync(cancellationToken),
            _ => throw new InvalidOperationException("Invalid playlist option selected")
        };
    }

    private async Task<int> SelectExistingPlaylistAsync(CancellationToken cancellationToken)
    {
        var playlists = await dbContext.Playlists
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        var selectedPlaylist = await AnsiConsole.PromptAsync(new SelectionPrompt<Playlist>()
            .Title("\nSearch for a playlist:")
            .PageSize(10)
            .EnableSearch()
            .AddChoices(playlists)
            .UseConverter(p => $"{p.Name} (Created: {p.CreatedAtUtc})"), cancellationToken);

        return selectedPlaylist.Id;
    }

    private async Task<int> CreateNewPlaylistAsync(CancellationToken cancellationToken)
    {
        AnsiConsole.WriteLine();
        var playlistName = await AnsiConsole.AskAsync<string>("Enter the name of the new playlist:", cancellationToken);

        Playlist playlist = new()
        {
            Name = playlistName
        };

        dbContext.Playlists.Add(playlist);

        await dbContext.SaveChangesAsync(cancellationToken);

        return playlist.Id;
    }
}
