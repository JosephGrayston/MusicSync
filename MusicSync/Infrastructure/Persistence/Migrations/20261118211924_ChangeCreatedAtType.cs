using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicSync.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCreatedAtType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAtUtc",
                table: "Playlists",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Playlists",
                newName: "CreatedAtUtc");
        }
    }
}
