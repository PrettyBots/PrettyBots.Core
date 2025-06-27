namespace PrettyBots.Environment.Model.Media;

public class VideoEntity : IMediaEntity
{
    public string? Caption { get; init; }
    
    public int Width { get; init; }
    public int Height { get; init; }
    public int Duration { get; init; }
    public string? FileName { get; set; }
    public string FileId { get; init; } = null!;    
    public MediaEntityType Type => MediaEntityType.Video;   
}