namespace PrettyBots.Environment.Model.Media;

public class VideoEntity : IMediaEntity
{
    public string? Caption { get; init; }
    public MediaEntityType Type => MediaEntityType.Video;
}