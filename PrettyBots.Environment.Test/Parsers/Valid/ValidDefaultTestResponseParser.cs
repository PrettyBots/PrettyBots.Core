using MorseCode.ITask;

using PrettyBots.Attributes.Parsers;
using PrettyBots.Environment.Test.Responses;

namespace PrettyBots.Environment.Test.Parsers.Valid;

[DefaultParser]
public class ValidDefaultTestResponseParser : BaseTestParser<TestResponse>
{
    protected override bool CanParse(TestMessage message) { throw new NotImplementedException(); }

    protected override ITask<TestResponse> ParseResponseAsync(TestMessage message, CancellationToken token = default) { throw new NotImplementedException(); }
}