using MorseCode.ITask;

using PrettyBots.Attributes.Parsers;
using PrettyBots.Tests.Environment.Messages;
using PrettyBots.Tests.Environment.Responses;

namespace PrettyBots.Tests.Environment.Parsers.Valid;

[DefaultParser]
public class ValidDefaultTestResponseParser : BaseTestParser<TestResponse>
{
    protected override bool CanParse(TestMessage message) { throw new NotImplementedException(); }

    protected override ITask<TestResponse> ParseResponseAsync(TestMessage message, CancellationToken token = default) { throw new NotImplementedException(); }
}