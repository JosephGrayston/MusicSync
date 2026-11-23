using MusicSync.Playlists;
using Shouldly;

namespace MusicSync.UnitTests.Playlists;

[TestClass]
public sealed class PlaylistNameValidatorTests
{
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void ValidateEmptyNullOrWhitespaceNameReturnsValidationResultOfEmpty(string name)
    {
        var result = PlaylistNameValidator.Validate(name, new HashSet<string>());
        
        result.ShouldBe(PlaylistNameValidationResult.Empty);
    }

    [TestMethod]
    public void ValidateExcessiveNameLengthReturnsValidationResultOfTooLong()
    {
        var name = new string('a', PlaylistNameValidator.MaxLength + 1);

        var result = PlaylistNameValidator.Validate(name, new HashSet<string>());

        result.ShouldBe(PlaylistNameValidationResult.TooLong);
    }

    [TestMethod]
    public void ValidateNameThatAlreadyExistsReturnsValidationResultOfAlreadyExists()
    {
        var existingNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "My Playlist" };

        var result = PlaylistNameValidator.Validate("my playlist", existingNames);

        result.ShouldBe(PlaylistNameValidationResult.AlreadyExists);
    }

    [TestMethod]
    public void ValidateValidNameReturnsValidationResultOfValid()
    {
        var result = PlaylistNameValidator.Validate("My Playlist", new HashSet<string>());

        result.ShouldBe(PlaylistNameValidationResult.Valid);
    }
}
