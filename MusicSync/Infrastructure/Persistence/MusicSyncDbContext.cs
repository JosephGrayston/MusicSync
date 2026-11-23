using Microsoft.EntityFrameworkCore;

namespace MusicSync.Infrastructure.Persistence;

public sealed class MusicSyncDbContext(DbContextOptions<MusicSyncDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(typeof(MusicSyncDbContext).Assembly);
}
