namespace PrettyBots.Environment.Model;

public class VideoEntity : IMediaEntity
{
    public string? Caption { get; init; }
    public MediaEntityType Type => MediaEntityType.Video;
}