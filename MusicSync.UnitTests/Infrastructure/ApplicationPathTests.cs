using MusicSync.Infrastructure;
using Shouldly;

namespace MusicSync.UnitTests.Infrastructure;

[TestClass]
public sealed class ApplicationPathTests
{
    [TestMethod]
    public void ConstructorDerivesPathsFromRoot()
    {
        var root = Path.Join(Path.GetTempPath(), "MusicSyncTests");

        var paths = new ApplicationPaths(root);

        paths.RootDirectory.ShouldBe(root);
        paths.DatabaseFile.ShouldBe(Path.Join(root, "musicsync.db"));
        paths.LogDirectory.ShouldBe(Path.Join(root, "logs"));
    }
}
