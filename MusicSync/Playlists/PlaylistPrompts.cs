using MusicSync.Infrastructure.Terminal;
using Spectre.Console;

namespace MusicSync.Playlists;

public sealed class PlaylistPrompts(IAnsiConsole console, DateTimeFormatter formatter) : IPlaylistPrompts
{
    public async Task<PlaylistAction> AskForPlaylistActionAsync(CancellationToken cancellationToken)
    {
        return await console.PromptAsync(new SelectionPrompt<PlaylistAction>()
            .Title("\nPlease select an option:")
            .AddChoices(PlaylistAction.SelectExistingPlaylist, PlaylistAction.CreateNewPlaylist)
            .UseConverter(action => action switch
            {
                PlaylistAction.SelectExistingPlaylist => "Select an existing playlist",
                PlaylistAction.CreateNewPlaylist => "Create a new playlist",
                _ => throw new ArgumentOutOfRangeException(nameof(action))
            }), cancellationToken);
    }

    public async Task<Playlist> ChoosePlaylistAsync(IReadOnlyList<Playlist> playlists, CancellationToken cancellationToken)
    {
        return await console.PromptAsync(new SelectionPrompt<Playlist>()
            .Title("\nSearch for a playlist:")
            .PageSize(10)
            .EnableSearch()
            .AddChoices(playlists)
            .UseConverter(p => Markup.Escape($"{p.Name} (Created: {formatter.ToDisplay(p.CreatedAt)})")), cancellationToken);
    }

    public async Task<string> AskForNewPlaylistNameAsync(IReadOnlySet<string> existingPlaylistNames, CancellationToken cancellationToken)
    {
        var playlistNamePrompt = new TextPrompt<string>("Enter the name of the new playlist:")
            .AllowEmpty()
            .Validate(name => PlaylistNameValidator.Validate(name, existingPlaylistNames) switch
            {
                PlaylistNameValidationResult.Empty => ValidationResult.Error("[red]Playlist name cannot be empty.[/]"),
                PlaylistNameValidationResult.TooLong => ValidationResult.Error($"[red]Playlist name is too long. Cannot exceed {PlaylistNameValidator.MaxLength} characters.[/]"),
                PlaylistNameValidationResult.AlreadyExists => ValidationResult.Error("[red]A playlist with that name already exists.[/]"),
                PlaylistNameValidationResult.Valid => ValidationResult.Success(),
                _ => throw new ArgumentOutOfRangeException(nameof(name), "Invalid playlist name validation result.")
            });

        return (await console.PromptAsync(playlistNamePrompt, cancellationToken)).Trim();
    }
}
