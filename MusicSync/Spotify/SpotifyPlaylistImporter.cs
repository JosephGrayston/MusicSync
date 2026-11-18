using Spectre.Console;

namespace MusicSync.Spotify;

public sealed class SpotifyPlaylistImporter(IAnsiConsole console, ISpotifyLauncher spotifyLauncher)
{
    public async Task ImportAsync(int playlistId)
    {
        console.Status()
            .Start("\nAttempting to open Spotify...", ctx =>
            {
                spotifyLauncher.OpenSpotify();
            });

        console.WriteLine("\nPlease login to your Spotify account and copy the playlist tracks to clipboard.");

        
    // Read clipboard, parse data and persist to database
    }
}
