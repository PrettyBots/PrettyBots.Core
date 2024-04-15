using PrettyBots.Environment;
using PrettyBots.Environment.Responses;
using PrettyBots.Interactions.Abstraction.Model.Descriptors;
using PrettyBots.Interactions.Abstraction.Model.Descriptors.Loading.Abstraction;
using PrettyBots.Interactions.Services;
using PrettyBots.Interactions.Validators.Abstraction;
using PrettyBots.Tests.Environment.Responses;
using PrettyBots.Tests.Environment.Validators.Configs;
using PrettyBots.Tests.Environment.Validators.Invalid;
using PrettyBots.Tests.Environment.Validators.Valid;

namespace PrettyBots.Tests.Loading;

[Order(2)]
[TestFixture]
[TestOf(typeof(EntitiesLoader))]
public class ValidatorTests : BaseLoadingTests
{
    private static readonly string[] _invalidValidatorNames = {
        nameof(InvalidConfigValidator),
        nameof(InvalidAbstractValidator),
        nameof(InvalidConfigValidator2),
        nameof(InvalidConstructorValidator),
        nameof(InvalidExceptionConstructorValidator),
        nameof(NeverLoadingValidator),
    };
    
    private static readonly string[] _validValidatorNames = {
        nameof(BasicValidator),
        nameof(BasicTypedValidator),
        nameof(ValidAcceptMultipleValidator),
        nameof(ValidAcceptAnyValidator),
        nameof(ValidGenericValidator),
    };
    
    [Test]
    public void TestLoading_NoSP_NoStrict()
    {
        InteractionService.Config.StrictLoadingModeEnabled = false;
        
        GenericMultipleLoadingResult<ResponseValidatorInfo> loadingResult = 
            InteractionService.Loader.LoadResponseValidators(EnvironmentAssembly);
        Assert.That(loadingResult.Loaded, Is.True);
        
        IEnumerable<string> failedParserNames = loadingResult.Entities!
            .Where(e => !e.Loaded)
            .Select(e => e.EntityName);
        
        IEnumerable<string> loadedParserNames = loadingResult.Entities!
             .Where(e => e.Loaded)
             .Select(e => e.EntityName);
        
        CollectionAssert.AreEquivalent(_invalidValidatorNames, failedParserNames);
        CollectionAssert.AreEquivalent(_validValidatorNames, loadedParserNames);

        // Registration test
        CollectionAssert.IsSubsetOf(_validValidatorNames,
            InteractionService.Registry.ResponseValidators.Values
                              .Select(v => v.ValidatorType.Name));

        ResponseValidatorInfo genericValidatorInfo = 
            InteractionService.Registry.ResponseValidators[typeof(ValidGenericValidator)];
        ResponseValidatorInfo acceptAnyValidatorInfo = 
            InteractionService.Registry.ResponseValidators[typeof(ValidAcceptAnyValidator)];
        ResponseValidatorInfo acceptMultipleValidatorInfo = 
            InteractionService.Registry.ResponseValidators[typeof(ValidAcceptMultipleValidator)];
        
        Assert.That(genericValidatorInfo.ServiceProvider, Is.Null);
        Assert.That(genericValidatorInfo.RegisteredInTheSP, Is.False);
        Assert.That(genericValidatorInfo.ResponseType.IsEquivalentTo(typeof(TestResponse)));
        Assert.That(genericValidatorInfo.AvailableConfigTypes, Has.Count.EqualTo(1));
        Assert.That(genericValidatorInfo.AvailableConfigTypes[0]
                        .IsEquivalentTo(typeof(IValidatorConfig<TestResponse>)));
        
        Assert.That(acceptAnyValidatorInfo.ServiceProvider, Is.Null);
        Assert.That(acceptAnyValidatorInfo.RegisteredInTheSP, Is.False);
        Assert.That(acceptAnyValidatorInfo.ResponseType.IsEquivalentTo(typeof(TextResponse)));
        Assert.That(acceptAnyValidatorInfo.AvailableConfigTypes, Has.Count.EqualTo(1));
        Assert.That(acceptAnyValidatorInfo.AvailableConfigTypes[0]
                        .IsEquivalentTo(typeof(IValidatorConfig<IUserResponse>)));
        
        Assert.That(acceptMultipleValidatorInfo.ServiceProvider, Is.Null);
        Assert.That(acceptMultipleValidatorInfo.RegisteredInTheSP, Is.False);
        Assert.That(acceptMultipleValidatorInfo.ResponseType.IsEquivalentTo(typeof(IAbstractResponse)));
        Assert.That(acceptMultipleValidatorInfo.AvailableConfigTypes, Has.Count.EqualTo(2));
        CollectionAssert.AreEquivalent(new[] {
            typeof(TestGeneralConfig),
            typeof(AbstractConfigImpl),
        }, acceptMultipleValidatorInfo.AvailableConfigTypes);

        GenericLoadingResult<ResponseValidatorInfo> notParserLoadingResult =
            InteractionService.Loader.LoadResponseValidator(typeof(AbstractConfigImpl));
        GenericLoadingResult<ResponseValidatorInfo> singleParserLoadingResult =
            InteractionService.Loader.LoadResponseValidator<TextResponse, ValidAcceptAnyValidator>();

        Assert.That(notParserLoadingResult.Loaded, Is.False);
        Assert.That(singleParserLoadingResult.Loaded, Is.False);
    }
}