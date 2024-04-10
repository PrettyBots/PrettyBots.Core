using PrettyBots.Environment.Parsers;
using PrettyBots.Tests.Environment.Responses;

namespace PrettyBots.Tests.Environment.Parsers.Generic;

public interface IGenericParser<out TParser> : IResponseParser<TParser>
    where TParser : IAbstractResponse
{
    
}