using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PhotoSync.ViewModels;

namespace PhotoSync.Tests.ViewModels;

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
