using MusicSync.Commands;

namespace MusicSync.Menu
{
    public record MenuOption(string Description, IPlaylistCommand? Command);
}
