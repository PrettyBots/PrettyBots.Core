using MorseCode.ITask;

using PrettyBots.Environment.Responses;
using PrettyBots.Environment.Test.Parsers.Invalid;

namespace PrettyBots.Environment.Test.Parsers.Valid;

public class ValidInheritParser : AbstractParser
{
    public override string Test => String.Empty;
    protected override bool CanParse(TestMessage message) { throw new NotImplementedException(); }

    protected override ITask<TextResponse> ParseResponseAsync(TestMessage message, CancellationToken token = default) { throw new NotImplementedException(); }
}