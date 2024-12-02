using MorseCode.ITask;

using PrettyBots.Environment.Parsers.Model;
using PrettyBots.Environment.Responses;
using PrettyBots.Tests.Environment.Messages;

namespace PrettyBots.Tests.Environment.Parsers.Valid;

public class ValidImageParser : BaseTestParser<ImageResponse>
{
    protected override bool CanParse(TestMessage message) { throw new NotImplementedException(); }

    protected override ITask<ParsingResult> ParseResponseAsync(TestMessage message, CancellationToken token = default) { throw new NotImplementedException(); }
}