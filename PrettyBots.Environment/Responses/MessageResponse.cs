namespace PrettyBots.Environment.Responses;

public class MessageResponse : IUserResponse
{
    public IEnvironment Environment { get; set; } = null!;
}