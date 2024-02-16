using PhotoSync.Domain.Entities;

namespace PhotoSync.Domain.Tests.Entities;

public class SourceFolderTests
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
    public void ExistsInExcludedFolders_Theories(bool expected, string pathToFind)
    {
        var data = this.GetSampleData();
        var sut = SourceFolder.Create("");
        foreach (var item in data)
        {
            sut.AddExcludedFolder(item);
        }

        var actual = sut.ExistsInExcludedFolders(pathToFind);
        Assert.Equal(expected, actual);
    }

    private IEnumerable<string> GetSampleData()
        => [
            "a\\b",
            "a\\b\\c"
        ];
}
