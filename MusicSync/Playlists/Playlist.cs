namespace MusicSync.Playlists;

public class Playlist
{
    public int Id { get; init; }

    public required string Name { get; init; }

    public required DateTimeOffset CreatedAt { get; init; } 
}
