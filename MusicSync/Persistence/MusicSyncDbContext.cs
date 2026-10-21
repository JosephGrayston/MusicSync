using Microsoft.EntityFrameworkCore;

namespace MusicSync.Persistence;

public sealed class MusicSyncDbContext(DbContextOptions<MusicSyncDbContext> options) : DbContext(options)
{
    public DbSet<Playlist> Playlists => Set<Playlist>();
}
