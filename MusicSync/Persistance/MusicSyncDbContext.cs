using Microsoft.EntityFrameworkCore;

namespace MusicSync.Data;

public class MusicSyncDbContext : DbContext
{
    public DbSet<Playlist> Playlists => Set<Playlist>();

    public string DbPath { get; }

    public MusicSyncDbContext(DbContextOptions<MusicSyncDbContext> options)
        : base (options)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);

        var musicSyncPath = Path.Join(path, "MusicSync");

        Directory.CreateDirectory(musicSyncPath);

        DbPath = Path.Join(musicSyncPath, "musicsync.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite($"Data Source={DbPath}");
}
