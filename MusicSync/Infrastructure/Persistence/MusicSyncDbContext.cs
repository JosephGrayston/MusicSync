using Microsoft.EntityFrameworkCore;
using MusicSync.Playlists;

namespace MusicSync.Infrastructure.Persistence;

public sealed class MusicSyncDbContext(DbContextOptions<MusicSyncDbContext> options) : DbContext(options)
{
    public DbSet<Playlist> Playlists => Set<Playlist>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Playlist>(playlist =>
        {
            playlist.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200)
                .UseCollation("NOCASE");

            playlist.HasIndex(p => p.Name).IsUnique();

            playlist.Property(e => e.CreatedAt)
                .IsRequired();
        });
    }
}
