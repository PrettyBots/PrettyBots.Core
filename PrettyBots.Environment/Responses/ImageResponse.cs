namespace PrettyBots.Environment.Responses;

public class ImageResponse : IUserResponse
{
    public IEnvironment Environment { get; set; }

    public ImageResponse(IEnvironment environment)
    {
        Environment = environment;
    }
}