using PrettyBots.Environment.Parsers;

namespace PrettyBots.Environment.Test.Parsers;

public abstract class BaseTestParser<TResponse> : ResponseParser<TestMessage, TResponse>
    where TResponse : IUserResponse
{
    
}