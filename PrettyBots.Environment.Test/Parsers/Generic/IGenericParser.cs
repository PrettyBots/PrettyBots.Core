using PrettyBots.Environment.Parsers;
using PrettyBots.Environment.Test.Responses;

namespace PrettyBots.Environment.Test.Parsers.Generic;

public interface IGenericParser<out TParser> : IResponseParser<TParser>
    where TParser : IAbstractResponse
{
    
}