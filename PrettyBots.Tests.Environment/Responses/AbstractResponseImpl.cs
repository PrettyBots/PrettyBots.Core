using PrettyBots.Environment;

namespace PrettyBots.Tests.Environment.Responses;

public class AbstractResponseImpl : IAbstractResponse
{
    public IEnvironment Environment { get; set; } = null!;
}