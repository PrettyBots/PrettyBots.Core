using MorseCode.ITask;

using PrettyBots.Environment.Parsers;
using PrettyBots.Tests.Environment.Messages;
using PrettyBots.Tests.Environment.Responses;

namespace PrettyBots.Tests.Environment.Parsers.Invalid;

public class InvalidMessageParser : ResponseParser<UnusedMessage, TestResponse>
{
    protected override bool CanParse(UnusedMessage message) { throw new NotImplementedException(); }

    protected override ITask<TestResponse> ParseResponseAsync(UnusedMessage message, CancellationToken token = default) { throw new NotImplementedException(); }
}