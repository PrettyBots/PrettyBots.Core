namespace PrettyBots.Environment.Test.Parsers.Valid;

public class ValidOverrideParser : ValidTextParser
{
    protected override bool CanParse(TestMessage message)
    {
        throw new NotImplementedException();
    }
}