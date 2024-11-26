using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using PrettyBots.Environment.Model;

namespace PrettyBots.Environment.Utilities;

public class MediaEntityConverter : JsonConverter<IMediaEntity>
{
    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, IMediaEntity? value, JsonSerializer serializer)
    {
        writer.WriteValue(value);
    }

    public override IMediaEntity? ReadJson(JsonReader reader, Type objectType, IMediaEntity? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject jObject = JObject.Load(reader);
        switch ((MediaEntityType?)jObject[nameof(IMediaEntity.Type)]?.Value<int>()) {
            case MediaEntityType.Photo:
                PhotoEntity photoEntity = new PhotoEntity();
                serializer.Populate(jObject.CreateReader(), photoEntity);
                return photoEntity;

            case MediaEntityType.Video:
                break;

            case null:
                return null;

            default:
                return null;
        }
        throw new NotImplementedException();
    }
    
}