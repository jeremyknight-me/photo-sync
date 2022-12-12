namespace PhotoSync.Common;

public class Photo
{
    public string RelativePath { get; set; }
    public PhotoAction ProcessAction { get; set; } = PhotoAction.New;
}
