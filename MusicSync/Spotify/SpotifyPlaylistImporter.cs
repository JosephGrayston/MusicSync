using Spectre.Console;

namespace MusicSync.Spotify;

public sealed class SpotifyPlaylistImporter(IAnsiConsole console, ISpotifyLauncher spotifyLauncher)
{
    public async Task ImportAsync(int playlistId)
    {
        var launchResult = spotifyLauncher.Open();

        console.MarkupLine(launchResult switch
        {
            SpotifyLauncher.SpotifyLaunchResult.Failed => "[red]Failed to open Spotify. Please ensure Spotify is installed or accessible via web.[/]",
            _ => "[green]Spotify opened successfully.[/]"
        });

        console.WriteLine("\nPlease login to your Spotify account and copy the playlist tracks to clipboard.");

        
    // Read clipboard, parse data and persist to database
    }
}
