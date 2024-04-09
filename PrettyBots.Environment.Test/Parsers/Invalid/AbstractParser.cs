using MorseCode.ITask;

using PrettyBots.Environment.Responses;

namespace PrettyBots.Environment.Test.Parsers.Invalid;

public abstract class AbstractParser : BaseTestParser<TextResponse>
{
    public abstract string Test { get; } 
}