using PrettyBots.Tests.Environment.Messages;

namespace PrettyBots.Tests.Environment.Parsers.Valid;

public class ValidOverrideParser : ValidTextParser
{
    protected override bool CanParse(TestMessage message)
    {
        throw new NotImplementedException();
    }
}