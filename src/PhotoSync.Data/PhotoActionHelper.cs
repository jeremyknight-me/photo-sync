using System.Collections.Generic;
using System.Linq;

namespace PhotoSync.Data
{
    public static class PhotoActionHelper
    {
        public static IEnumerable<KeyValuePair<int, string>> MakeEnumerable()
            => new List<PhotoAction> { PhotoAction.New, PhotoAction.Sync, PhotoAction.Ignore }
                .Select(x => new KeyValuePair<int, string>((int)x, x.ToString()))
                .OrderBy(x => x.Key);
    }
}
