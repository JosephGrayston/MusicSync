using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace MusicSync.Spotify;

public sealed partial class SpotifyLauncher(ILogger<SpotifyLauncher> logger) : ISpotifyLauncher
{
    public SpotifyLaunchResult Open()
    {
        if (TryStart("spotify:")) return SpotifyLaunchResult.OpenedApp;
        if (TryStart("https://open.spotify.com/")) return SpotifyLaunchResult.OpenedWeb;

        return SpotifyLaunchResult.Failed;
    }

    private bool TryStart(string target) 
    {
        try
        {
            using var _ = Process.Start(new ProcessStartInfo(target) { UseShellExecute = true });
            return true;
        }
        catch (Exception ex)
        {
            SpotifyLauncherLog.SpotifyLaunchWarning(logger, ex, target);
            return false;
        }
    }
}

public static partial class SpotifyLauncherLog
{
    [LoggerMessage(Level = LogLevel.Warning, Message = "Failed to open Spotify: {Target}")]
    public static partial void SpotifyLaunchWarning(ILogger logger, Exception ex, string target);
}