using PhotoSync.Domain.Entities;
using PhotoSync.Domain.Extensions;
using PhotoSync.Domain.ValueObjects;

namespace PhotoSync.Domain.Tests.Extensions;

public class ExcludedFoldersExtensionsTests
{
    [Theory]
    [InlineData(false, null)]
    [InlineData(false, "")]
    [InlineData(false, " ")]
    [InlineData(false, "a")]
    [InlineData(false, "a\\1.png")]
    [InlineData(false, "b")]
    [InlineData(false, "b\\2.png")]
    [InlineData(false, "c")]
    [InlineData(false, "c\\3.png")]
    [InlineData(false, "b\\c")]
    [InlineData(false, "b\\c\\23.png")]
    [InlineData(true, "a\\b\\c")]
    [InlineData(true, "a\\b\\c\\123.png")]
    [InlineData(true, "a\\b")]
    [InlineData(true, "a\\b\\12.png")]
    public void Exists_Theories(bool expected, string pathToFind)
    {
        var data = this.GetSampleData();
        var actual = data.Exists(pathToFind);
        Assert.Equal(expected, actual);
    }

    private IEnumerable<ExcludedFolder> GetSampleData()
        => new List<ExcludedFolder>
        {
            ExcludedFolder.Create(SourceFolderId.New(), "a\\b"),
            ExcludedFolder.Create(SourceFolderId.New(), "a\\b\\c")
        };
}
