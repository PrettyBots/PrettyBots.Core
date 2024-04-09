using MorseCode.ITask;

using PrettyBots.Environment.Parsers;
using PrettyBots.Environment.Responses;
using PrettyBots.Environment.Test.Responses;

namespace PrettyBots.Environment.Test.Parsers.Generic;

public class ValidGenericParser : ResponseParser<TestMessage, AdditionalResponse>, 
                                  IGenericParser<AdditionalResponse>
{
    protected override bool CanParse(TestMessage message) { throw new NotImplementedException(); }

    protected override ITask<AdditionalResponse> ParseResponseAsync(TestMessage message, CancellationToken token = default) { throw new NotImplementedException(); }
}