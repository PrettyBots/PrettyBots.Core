using MorseCode.ITask;

using PrettyBots.Attributes.Common;
using PrettyBots.Tests.Environment.Messages;
using PrettyBots.Tests.Environment.Responses;

namespace PrettyBots.Tests.Environment.Parsers.Invalid;

[Ignore]
public class NeverLoadingParser : BaseTestParser<TestResponse>
{
    protected override bool CanParse(TestMessage message) { throw new NotImplementedException(); }

    protected override ITask<TestResponse> ParseResponseAsync(TestMessage message, CancellationToken token = default) { throw new NotImplementedException(); }
}