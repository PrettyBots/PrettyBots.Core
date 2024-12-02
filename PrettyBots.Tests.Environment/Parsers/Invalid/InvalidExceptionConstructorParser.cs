using MorseCode.ITask;

using PrettyBots.Environment.Parsers.Model;
using PrettyBots.Tests.Environment.Messages;
using PrettyBots.Tests.Environment.Responses;

namespace PrettyBots.Tests.Environment.Parsers.Invalid;

public class InvalidExceptionConstructorParser : BaseTestParser<TestResponse>
{
    public InvalidExceptionConstructorParser()
    {
        throw new IgnorableException();
    }
    
    protected override bool CanParse(TestMessage message) { throw new NotImplementedException(); }

    protected override ITask<ParsingResult> ParseResponseAsync(TestMessage message, CancellationToken token = default) { throw new NotImplementedException(); }
}