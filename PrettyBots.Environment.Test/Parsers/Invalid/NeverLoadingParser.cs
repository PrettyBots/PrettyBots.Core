using MorseCode.ITask;

using PrettyBots.Attributes.Common;
using PrettyBots.Environment.Test.Responses;

namespace PrettyBots.Environment.Test.Parsers.Invalid;

[Ignore]
public class NeverLoadingParser : BaseTestParser<TestResponse>
{
    protected override bool CanParse(TestMessage message) { throw new NotImplementedException(); }

    protected override ITask<TestResponse> ParseResponseAsync(TestMessage message, CancellationToken token = default) { throw new NotImplementedException(); }
}