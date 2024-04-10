using PrettyBots.Environment;
using PrettyBots.Tests.Environment.Messages;

namespace PrettyBots.Tests.Environment;

public class TestEnvironment : IEnvironment
{
    public readonly static TestEnvironment Instance = new TestEnvironment();
    
    public Type MessageType => typeof(TestMessage);
}