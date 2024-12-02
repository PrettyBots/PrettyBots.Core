using MorseCode.ITask;

using PrettyBots.Environment.Parsers.Model;
using PrettyBots.Environment.Responses;
using PrettyBots.Environment.Responses.TextBased;
using PrettyBots.Tests.Environment.Messages;
using PrettyBots.Tests.Environment.Parsers.Invalid;

namespace PrettyBots.Tests.Environment.Parsers.Valid;

public class ValidInheritParser : AbstractParser
{
    public override string Test => String.Empty;
    protected override bool CanParse(TestMessage message) { throw new NotImplementedException(); }

    protected override ITask<ParsingResult> ParseResponseAsync(TestMessage message, CancellationToken token = default) { throw new NotImplementedException(); }
}