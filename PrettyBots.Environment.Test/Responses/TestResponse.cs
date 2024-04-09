namespace PrettyBots.Environment.Test.Responses;

public class TestResponse : IAbstractResponse
{
    public IEnvironment Environment { get; set; } = new TestEnvironment();
}