using Microsoft.Extensions.Logging;

namespace MusicSync.Logging;

internal static partial class MusicSyncLog
{
    [LoggerMessage(Level = LogLevel.Critical, Message = "MusicSync terminated unexpectedly.")]
    public static partial void CritialApplicationError(ILogger logger, Exception ex);
}
