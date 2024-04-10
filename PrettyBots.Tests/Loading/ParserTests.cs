using PrettyBots.Environment.Responses;
using PrettyBots.Interactions.Services;
using PrettyBots.Model.Descriptors;
using PrettyBots.Model.Descriptors.Loading.Abstraction;
using PrettyBots.Tests.Environment.Messages;
using PrettyBots.Tests.Environment.Parsers;
using PrettyBots.Tests.Environment.Parsers.Generic;
using PrettyBots.Tests.Environment.Parsers.Invalid;
using PrettyBots.Tests.Environment.Parsers.Valid;

namespace PrettyBots.Tests.Loading;

[Order(1)]
[TestFixture]
[TestOf(typeof(EntitiesLoader))]
[TestOf(typeof(LoadedEntitiesRegistry))]
public class ParserTests : BaseLoadingTests
{
    private static readonly string[] _validParserNames = {
        nameof(ValidInheritParser),
        nameof(ValidTextParser),
        nameof(ValidOverrideParser),
        nameof(ValidTestResponseParser),
        nameof(ValidDefaultTestResponseParser),
        nameof(ValidAdditionalParser),
        nameof(ValidGenericParser),
    };
    
    private static readonly string[] _invalidParserNames = {
        nameof(NeverLoadingParser),
        nameof(InvalidConstructorParser),
        nameof(InvalidExceptionConstructorParser),
        nameof(InvalidGenericParser),
        nameof(AbstractParser),
        nameof(InvalidInheritanceParser),
        nameof(InvalidMessageParser),
        typeof(BaseTestParser<>).Name,
    };
    
    [Test]
    public void TestParsersLoading_NoSP_NoStrict()
    {
        InteractionService.Config.StrictLoadingModeEnabled = false;
        
        GenericMultipleLoadingResult<ResponseParserInfo> loadingResult = 
            InteractionService.Loader.LoadResponseParsers(EnvironmentAssembly);
        
        Assert.That(loadingResult.Loaded, Is.True);
        
        IEnumerable<string> failedParserNames = loadingResult.Entities!
            .Where(e => !e.Loaded)
            .Select(e => e.EntityName);
        
        IEnumerable<string> loadedParserNames = loadingResult.Entities!
             .Where(e => e.Loaded)
             .Select(e => e.EntityName);
        
        CollectionAssert.AreEquivalent(_invalidParserNames, failedParserNames);
        CollectionAssert.AreEquivalent(_validParserNames, loadedParserNames);

        GenericLoadingResult<ResponseParserInfo> singleParserLoadingResult =
            InteractionService.Loader.LoadResponseParser<TestMessage, TextResponse, ValidTextParser>();
        
        Assert.That(singleParserLoadingResult.Loaded, Is.False);
        
        // Test registry
        CollectionAssert.IsSubsetOf(_validParserNames,
            InteractionService.Registry.ResponseParsers.Values
                .SelectMany(p => p)
                .Select(p => p.ParserType.Name));
    }
}