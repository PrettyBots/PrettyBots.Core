using PrettyBots.Environment;
using PrettyBots.Environment.Parsers;
using PrettyBots.Tests.Environment.Messages;

namespace PrettyBots.Tests.Environment.Parsers;

public abstract class BaseTestParser<TResponse> : ResponseParser<TestMessage, TResponse>
    where TResponse : class, IUserResponse
{
    
}