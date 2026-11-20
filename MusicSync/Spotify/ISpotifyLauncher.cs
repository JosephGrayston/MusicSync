using static MusicSync.Spotify.SpotifyLauncher;

namespace MusicSync.Spotify;

public interface ISpotifyLauncher
{
    SpotifyLaunchResult Open();
}
