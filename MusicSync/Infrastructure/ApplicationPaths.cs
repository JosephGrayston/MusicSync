namespace MusicSync.Infrastructure;

public sealed class ApplicationPaths
{
    public ApplicationPaths(string rootDirectory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rootDirectory);

        RootDirectory = rootDirectory;
        DatabaseFile = Path.Join(RootDirectory, "musicsync.db");
        LogDirectory = Path.Join(RootDirectory, "log");
    }

    public string RootDirectory { get; init; }

    public string DatabaseFile { get; init; }

    public string LogDirectory { get; init; }

    public static ApplicationPaths ForCurrentUser()
    {
        var localDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        // TO DO: Consider using a more robust method to determine the local application data folder, especially for cross-platform compatibility.
        if (string.IsNullOrWhiteSpace(localDataPath))
        {
            throw new InvalidOperationException("Could not determine the local application data folder for the current user.");
        }

        return new ApplicationPaths(Path.Join(localDataPath, "MusicSync"));
    }

    public void EnsureDirectoriesCreated()
    {
        Directory.CreateDirectory(RootDirectory);
        Directory.CreateDirectory(LogDirectory);
    }
}
