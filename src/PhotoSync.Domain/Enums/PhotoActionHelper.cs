using System.Collections.ObjectModel;

namespace PhotoSync.Domain.Enums;

public static class PhotoActionHelper
{
    public static IReadOnlyDictionary<int, string> MakeDictionary()
    {
        var actions = new Dictionary<int, string>()
        {
            { (int)PhotoAction.New, PhotoAction.New.ToString() },
            { (int)PhotoAction.Sync, PhotoAction.Sync.ToString() },
            { (int)PhotoAction.Ignore, PhotoAction.Ignore.ToString() }
        };
        return new ReadOnlyDictionary<int, string>(actions);
    }
}
