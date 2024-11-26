namespace PrettyBots.Environment.Model;

public class PhotoEntitySize
{
    public int Width { get; init; }
    
    public int Height { get; init; }

    public string FileId { get; init; } = null!;

    public long? FileSize { get; init; }
}   