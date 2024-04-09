namespace PrettyBots.Environment.Test;

public class TestEnvironment : IEnvironment
{
    public Type MessageType => typeof(TestMessage);
}