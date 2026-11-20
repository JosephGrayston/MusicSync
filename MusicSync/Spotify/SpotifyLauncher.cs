using Microsoft.Extensions.Logging;
using MusicSync.Infrastructure.Logging;
using System.ComponentModel;
using System.Diagnostics;

namespace MusicSync.Spotify;

public sealed class SpotifyLauncher(ILogger<SpotifyLauncher> logger) : ISpotifyLauncher
{
    public enum SpotifyLaunchResult
    {
        OpenedApp,
        OpenedWeb,
        Failed
    }

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
        catch (Win32Exception ex)
        {
            MusicSyncLog.SpotifyLaunchError(logger, ex);
            return false;
        }
    }
}
