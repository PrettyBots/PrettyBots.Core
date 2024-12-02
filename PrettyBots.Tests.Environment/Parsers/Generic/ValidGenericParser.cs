using MorseCode.ITask;

using PrettyBots.Environment.Parsers;
using PrettyBots.Environment.Parsers.Model;
using PrettyBots.Tests.Environment.Messages;
using PrettyBots.Tests.Environment.Responses;

namespace PrettyBots.Tests.Environment.Parsers.Generic;

public class ValidGenericParser : ResponseParser<TestMessage, AbstractResponseImpl>, 
                                  IGenericParser<AbstractResponseImpl>
{
    protected override bool CanParse(TestMessage message) { throw new NotImplementedException(); }

    protected override ITask<ParsingResult> ParseResponseAsync(TestMessage message, CancellationToken token = default) { throw new NotImplementedException(); }
}