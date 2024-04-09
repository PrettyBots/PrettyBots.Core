using MorseCode.ITask;

using PrettyBots.Environment.Test.Responses;

namespace PrettyBots.Environment.Test.Parsers.Valid;

public class ValidAdditionalParser : BaseTestParser<AdditionalResponse>
{
    protected override bool CanParse(TestMessage message) { throw new NotImplementedException(); }

    protected override ITask<AdditionalResponse> ParseResponseAsync(TestMessage message, CancellationToken token = default) { throw new NotImplementedException(); }
}