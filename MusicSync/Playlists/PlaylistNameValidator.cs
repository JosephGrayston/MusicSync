namespace MusicSync.Playlists;

public static class PlaylistNameValidator
{
    public const int MaxLength = 200;

    public static PlaylistNameValidationResult Validate(string? name, IReadOnlySet<string> existingNames)
    {
        var trimmedName = name?.Trim();

        if (string.IsNullOrWhiteSpace(trimmedName)) return PlaylistNameValidationResult.Empty;

        if (trimmedName.Length > MaxLength) return PlaylistNameValidationResult.TooLong;

        return existingNames.Contains(trimmedName, StringComparer.OrdinalIgnoreCase)
            ? PlaylistNameValidationResult.AlreadyExists
            : PlaylistNameValidationResult.Valid;
    }
}

public enum PlaylistNameValidationResult 
{ 
    Valid,
    TooLong,
    AlreadyExists,
    Empty,
}
