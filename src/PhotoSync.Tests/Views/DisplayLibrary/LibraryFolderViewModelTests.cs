using PhotoSync.Views.DisplayLibrary;

namespace PhotoSync.Tests.Views.DisplayLibrary;

public class LibraryFolderViewModelTests
{
    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public void IsExcluded_NoParent_Theories(bool expected, bool value)
    {
        var sut = new LibraryFolderViewModel
        {
            IsExcluded = value,
            Parent = null
        };
        var actual = sut.IsExcluded;
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(false, false, false)]
    [InlineData(true, false, true)]
    [InlineData(true, true, false)]
    [InlineData(true, true, true)]
    public void IsExcluded_Parent_Theories(bool expected, bool parentValue, bool childValue)
    {
        var parent = new LibraryFolderViewModel
        {
            IsExcluded = parentValue,
            Parent = null
        };
        var sut = new LibraryFolderViewModel
        {
            IsExcluded = childValue,
            Parent = parent
        };
        var actual = sut.IsExcluded;
        Assert.Equal(expected, actual);
    }
}
