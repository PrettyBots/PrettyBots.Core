using System.Text.Json.Serialization;

namespace PrettyBots.Environment.Model;

public class PhotoEntity : IMediaEntity
{
    public string? Caption { get; init; }
    public MediaEntityType Type => MediaEntityType.Photo;
    public List<PhotoEntitySize> Sizes { get; init; } = null!;
}