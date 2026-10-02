namespace MusicSync.Spotify;

public class SpotifyLaunchException(string message, Exception innerException) : Exception(message, innerException);
