using Microsoft.Extensions.Logging;

namespace MusicSync.Infrastructure.Logging;

public static partial class MusicSyncLog
{
    [LoggerMessage(Level = LogLevel.Critical, Message = "MusicSync terminated unexpectedly.")]
    public static partial void CriticalApplicationError(ILogger logger, Exception ex);

    [LoggerMessage(Level = LogLevel.Error, Message = "Failed to open Spotify.")]
    public static partial void SpotifyLaunchError(ILogger logger, Exception ex);
}
