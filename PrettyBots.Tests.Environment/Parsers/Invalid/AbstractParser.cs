using PrettyBots.Environment.Responses;
using PrettyBots.Environment.Responses.TextBased;

namespace PrettyBots.Tests.Environment.Parsers.Invalid;

public abstract class AbstractParser : BaseTestParser<TextResponse>
{
    public abstract string Test { get; } 
}