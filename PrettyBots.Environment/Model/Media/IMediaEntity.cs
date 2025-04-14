using Newtonsoft.Json;

using PrettyBots.Environment.Utilities;

namespace PrettyBots.Environment.Model.Media;

[JsonConverter(typeof(MediaEntityConverter))]
public interface IMediaEntity
{
    string? Caption { get; init; }

    MediaEntityType Type { get; }
}