namespace PrettyBots.Environment.Test.Responses;

public class AdditionalResponse : IAbstractResponse
{
    public IEnvironment Environment { get; set; } = new TestEnvironment();
}