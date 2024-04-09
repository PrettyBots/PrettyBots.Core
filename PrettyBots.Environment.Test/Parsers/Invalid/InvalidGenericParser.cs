using MorseCode.ITask;

namespace PrettyBots.Environment.Test.Parsers.Invalid;

/// <summary>
/// Tests invalid parser that does not use implementation
/// of the user response as the type parameter.
/// </summary>
public class InvalidGenericParser : BaseTestParser<IUserResponse>
{
    protected override bool CanParse(TestMessage message) { throw new NotImplementedException(); }

    protected override ITask<IUserResponse> ParseResponseAsync(TestMessage message, CancellationToken token = default) { throw new NotImplementedException(); }
}