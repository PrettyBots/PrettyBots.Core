using PrettyBots.Environment;

namespace PrettyBots.Tests.Environment.Responses;

public class TestResponse : IUserResponse
{
    public IEnvironment Environment { get; set; } = null!;
}