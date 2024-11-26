using PrettyBots.Environment.Model;

namespace PrettyBots.Environment.Responses.Media;

public class MediaResponse : IUserResponse
{
    public IEnvironment Environment { get; set; } = null!;
    public IMediaEntity MediaEntity { get; set; } = null!;
}