using PrettyBots.Environment.Responses;

namespace PrettyBots.Tests.Environment.Parsers.Invalid;

public abstract class AbstractParser : BaseTestParser<TextResponse>
{
    public abstract string Test { get; } 
}