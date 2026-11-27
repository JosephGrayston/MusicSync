using Microsoft.Extensions.Time.Testing;
using MusicSync.Infrastructure.Terminal;
using Shouldly;
using System.Globalization;

namespace MusicSync.UnitTests.Infrastructure.Terminal;

[TestClass]
public sealed class DateTimeFormatterTests
{
    private CultureInfo _originalCulture = null!;

    [TestInitialize]
    public void SetCulture()
    {
        _originalCulture = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = new CultureInfo("en-GB"); 
    }

    [TestCleanup]
    public void RestoreCulture() => CultureInfo.CurrentCulture = _originalCulture;

    [TestMethod]
    public void ToDisplayConvertsToProviderLocalTimeZone()
    {
        var time = new FakeTimeProvider();
        time.SetLocalTimeZone(TimeZoneInfo.CreateCustomTimeZone("Test+5", TimeSpan.FromHours(5), "Test+5", "Test+5"));

        var result = new DateTimeFormatter(time).ToDisplay(new DateTimeOffset(2026, 11, 23, 9, 30, 0, TimeSpan.Zero));

        result.ShouldContain("14:30");
    }

    [TestMethod]
    public void ToDisplayAppliesDaylightSavingForTheDate()
    {
        var time = new FakeTimeProvider();
        time.SetLocalTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Europe/London"));
        var formatter = new DateTimeFormatter(time);

        formatter.ToDisplay(new DateTimeOffset(2026, 7, 1, 9, 30, 0, TimeSpan.Zero)).ShouldContain("10:30");   // BST
        formatter.ToDisplay(new DateTimeOffset(2026, 12, 1, 9, 30, 0, TimeSpan.Zero)).ShouldContain("09:30");  // GMT
    }
}
