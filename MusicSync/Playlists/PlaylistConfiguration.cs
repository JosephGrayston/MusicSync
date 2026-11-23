using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MusicSync.Playlists;

internal sealed class PlaylistConfiguration : IEntityTypeConfiguration<Playlist>
{
    public void Configure(EntityTypeBuilder<Playlist> playlist)
    {
        playlist.Property(p => p.Name)
            .HasMaxLength(PlaylistNameValidator.MaxLength)
            .UseCollation("NOCASE");

        playlist.HasIndex(p => p.Name).IsUnique();
    }
}
