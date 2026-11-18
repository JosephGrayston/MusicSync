namespace MusicSync.Playlists;

public interface IPlaylistStore
{
    Task<IReadOnlyList<Playlist>> GetAllAsync(CancellationToken cancellationToken);

    Task<Playlist> CreateAsync(string name, CancellationToken cancellationToken);
}
