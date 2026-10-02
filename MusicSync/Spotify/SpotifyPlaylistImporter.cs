using Spectre.Console;

namespace MusicSync.Spotify;

public class SpotifyPlaylistImporter(ISpotifyLauncher spotifyLauncher)
{
    public async Task ImportAsync(int playlistId)
    {
        AnsiConsole.Status()
            .Start("\nAttempting to open Spotify...", ctx =>
            {
                spotifyLauncher.TryOpenSpotify();
            });

        AnsiConsole.WriteLine("\nPlease login to your Spotify account and copy the playlist tracks to clipboard.");


    // Read clipboard, parse data and persist to database
    }
}
