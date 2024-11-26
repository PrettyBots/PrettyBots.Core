using PrettyBots.Environment.Model;

namespace PrettyBots.Environment.Responses.Media;

public class MediaGroupResponse : IUserResponse
{
    public IEnvironment Environment { get; set; } = null!;
    public List<IMediaEntity> MediaEntities { get; set; } = null!;
}