namespace PhotoSync.ValueObjects;

public sealed class StoredText
{
    private string text = null!;

    public string Text
    {
        get => this.text;
        set => this.text = value.Trim();
    }
}
