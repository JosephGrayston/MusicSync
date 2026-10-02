namespace MusicSync.Data;

public class Playlist
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
}
