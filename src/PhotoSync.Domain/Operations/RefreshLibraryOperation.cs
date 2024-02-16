using PhotoSync.Domain.Entities;

namespace PhotoSync.Domain.Operations;

public interface IRefreshLibraryOperation
{
    void Run(PhotoLibrary library);
}

public sealed class RefreshLibraryOperation : IRefreshLibraryOperation
{
    private readonly IRefreshSourceFolderOperation refreshSourceFolder;

    public RefreshLibraryOperation(IRefreshSourceFolderOperation refreshSourceFolderOperation)
    {
        this.refreshSourceFolder = refreshSourceFolderOperation;
    }

    public void Run(PhotoLibrary library)
    {
        foreach (var sourceFolder in library.SourceFolders)
        {
            this.refreshSourceFolder.Run(sourceFolder);
        }
    }    
}
