using System.Diagnostics;

namespace MusicSync.Spotify;

public sealed class SpotifyLauncher : ISpotifyLauncher
{
    public void TryOpenSpotify()
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "spotify:",
                UseShellExecute = true
            });
        }
        catch (Exception)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://open.spotify.com/",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                throw new SpotifyLaunchException("Unable to open Spotify. Please ensure that Spotify is installed on your system or access it via the web.", ex);
            }
        }
    }
}
