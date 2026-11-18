using Microsoft.EntityFrameworkCore;
using MusicSync.Infrastructure.Persistence;

namespace MusicSync.Playlists;

public sealed class PlaylistStore(MusicSyncDbContext dbContext, TimeProvider timeProvider) : IPlaylistStore
{
    public async Task<Playlist> CreateAsync(string name, CancellationToken cancellationToken)
    {
        var playlist = new Playlist
        {
            Name = name,
            CreatedAt = timeProvider.GetUtcNow()
        };

        dbContext.Playlists.Add(playlist);

        await dbContext.SaveChangesAsync(cancellationToken);

        return playlist;
    }

    public async Task<IReadOnlyList<Playlist>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Playlists
            .OrderBy(p => p.Name)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
