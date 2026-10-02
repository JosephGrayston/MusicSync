using Microsoft.EntityFrameworkCore;
using MusicSync.Data;
using Spectre.Console;

namespace MusicSync.Playlists;

public class PlaylistSelector(MusicSyncDbContext dbContext)
{
    private enum PlaylistOption
    {
        SelectExistingPlaylist,

        CreateNewPlaylist
    }

    public async Task<int> SelectAsync()
    {
        // TO DO: inject prompt class here
        var option = await AnsiConsole.PromptAsync(new SelectionPrompt<PlaylistOption>()
            .Title("\nPlease select a option:")
            .AddChoices(PlaylistOption.SelectExistingPlaylist, PlaylistOption.CreateNewPlaylist));

        return option switch
        {
            PlaylistOption.SelectExistingPlaylist => await SelectExistingPlaylistAsync(),
            PlaylistOption.CreateNewPlaylist => await CreateNewPlaylistAsync(),
            _ => throw new InvalidOperationException("Invalid playlist option selected")
        };
    }

    private async Task<int> SelectExistingPlaylistAsync()
    {
        var playlists = await dbContext.Playlists
            .OrderBy(p => p.Name)
            .ToListAsync();

        var selectedPlaylist = await AnsiConsole.PromptAsync(new SelectionPrompt<Playlist>()
            .Title("\nSearch for a playlist:")
            .PageSize(10)
            .EnableSearch()
            .AddChoices(playlists)
            .UseConverter(p => $"{p.Name} (Created: {p.CreatedAtUtc})"));

        return selectedPlaylist.Id;
    }

    private async Task<int> CreateNewPlaylistAsync()
    {
        AnsiConsole.WriteLine();
        var playlistName = await AnsiConsole.AskAsync<string>("Enter the name of the new playlist:");

        Playlist playlist = new()
        {
            Name = playlistName
        };

        dbContext.Playlists.Add(playlist);

        await dbContext.SaveChangesAsync();

        return playlist.Id;
    }
}
