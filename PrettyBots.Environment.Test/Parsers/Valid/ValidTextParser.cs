using MorseCode.ITask;

using PrettyBots.Environment.Responses;

namespace PrettyBots.Environment.Test.Parsers.Valid;

/// <summary>
/// Tests valid parser
/// </summary>
public class ValidTextParser : BaseTestParser<TextResponse>
{
    protected override bool CanParse(TestMessage message) { throw new NotImplementedException(); }

    protected override ITask<TextResponse> ParseResponseAsync(TestMessage message, CancellationToken token = default) { throw new NotImplementedException(); }
}