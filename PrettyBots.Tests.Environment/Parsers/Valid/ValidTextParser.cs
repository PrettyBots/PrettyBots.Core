using MorseCode.ITask;

using PrettyBots.Attributes.Parsers;
using PrettyBots.Environment.Parsers.Model;
using PrettyBots.Environment.Responses;
using PrettyBots.Environment.Responses.TextBased;
using PrettyBots.Tests.Environment.Messages;

namespace PrettyBots.Tests.Environment.Parsers.Valid;

/// <summary>
/// Tests valid parser
/// </summary>
[DefaultParser]
public class ValidTextParser : BaseTestParser<TextResponse>
{
    protected override bool CanParse(TestMessage message) { throw new NotImplementedException(); }

    protected override ITask<ParsingResult> ParseResponseAsync(TestMessage message, CancellationToken token = default) { throw new NotImplementedException(); }
}