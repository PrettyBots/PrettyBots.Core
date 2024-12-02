using MorseCode.ITask;

using PrettyBots.Environment;
using PrettyBots.Environment.Parsers;
using PrettyBots.Environment.Parsers.Model;
using PrettyBots.Tests.Environment.Responses;

namespace PrettyBots.Tests.Environment.Parsers.Invalid;

/// <summary>
/// Test parser that will not be detected and will throw an exception
/// during the explicit loading, due to not inherit
/// <see cref="ResponseParser{TMessage,TResponse}"/> class.
/// </summary>
public class InvalidInheritanceParser : IResponseParser<TestResponse>
{
    public bool CanParse(IUserMessage message) { throw new NotImplementedException(); }

    public ITask<ParsingResult> ParseResponseAsync(IUserMessage message, CancellationToken token = default) { throw new NotImplementedException(); }
}