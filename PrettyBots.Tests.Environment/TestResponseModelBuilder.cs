using PrettyBots.Environment;
using PrettyBots.Interactions.Builders.InteractionResponses;
using PrettyBots.Tests.Environment.Messages;

namespace PrettyBots.Tests.Environment;

public class TestResponseModelBuilder<TResponse> : ResponseModelBuilder<TestMessage, TResponse>
    where TResponse : class, IUserResponse, new()
{
    protected TestResponseModelBuilder(string key) : base(key)
    {
    }
}