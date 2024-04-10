using PrettyBots.Environment;

namespace PrettyBots.Tests.Environment.Responses;

public class AdditionalResponse : IAbstractResponse
{
    public IEnvironment Environment { get; set; } = null!;
}