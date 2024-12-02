using MorseCode.ITask;

using PrettyBots.Environment;
using PrettyBots.Environment.Parsers.Model;
using PrettyBots.Tests.Environment.Messages;

namespace PrettyBots.Tests.Environment.Parsers.Invalid;

/// <summary>
/// Tests invalid parser that does not use implementation
/// of the user response as the type parameter.
/// </summary>
public class InvalidGenericParser : BaseTestParser<IUserResponse>
{
    protected override bool CanParse(TestMessage message) { throw new NotImplementedException(); }

    protected override ITask<ParsingResult> ParseResponseAsync(TestMessage message, CancellationToken token = default) { throw new NotImplementedException(); }
}