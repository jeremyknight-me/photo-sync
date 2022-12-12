using System.Collections.Generic;
using System.Linq;

namespace PhotoSync.Enums;

public enum PhotoAction
{
    New = 0,
    Sync = 1,
    Ignore = 2
}

public static class PhotoActionHelper
{
    public static IEnumerable<KeyValuePair<int, string>> MakeEnumerable()
        => new List<PhotoAction> { PhotoAction.New, PhotoAction.Sync, PhotoAction.Ignore }
            .Select(x => new KeyValuePair<int, string>((int)x, x.ToString()))
            .OrderBy(x => x.Key);
}
