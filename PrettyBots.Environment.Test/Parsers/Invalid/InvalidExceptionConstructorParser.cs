using MorseCode.ITask;

using PrettyBots.Environment.Test.Responses;

namespace PrettyBots.Environment.Test.Parsers.Invalid;

public class InvalidExceptionConstructorParser : BaseTestParser<TestResponse>
{
    public InvalidExceptionConstructorParser()
    {
        throw new IgnorableException();
    }
    
    protected override bool CanParse(TestMessage message) { throw new NotImplementedException(); }

    protected override ITask<TestResponse> ParseResponseAsync(TestMessage message, CancellationToken token = default) { throw new NotImplementedException(); }
}