namespace MusicSync.Playlists;

public static class PlaylistNameValidator
{
    public const int MaxLength = 200;

    public static PlaylistNameValidationResult Validate(string? name, IEnumerable<string> existingNames)
    {
        var trimmedName = name?.Trim();

        if (string.IsNullOrEmpty(trimmedName)) return PlaylistNameValidationResult.Empty;

        if (trimmedName.Length > MaxLength) return PlaylistNameValidationResult.TooLong;

        return existingNames.Contains(trimmedName, StringComparer.OrdinalIgnoreCase)
            ? PlaylistNameValidationResult.AlreadyExists
            : PlaylistNameValidationResult.None;
    }
}

public enum PlaylistNameValidationResult 
{ 
    Empty,
    TooLong,
    AlreadyExists,
    None
}

