using PrettyBots.Environment.Responses;
using PrettyBots.Interactions.Abstraction.Model;
using PrettyBots.Interactions.Abstraction.Model.Descriptors.Loading.Abstraction;
using PrettyBots.Interactions.Abstraction.Model.Responses;
using PrettyBots.Interactions.Services;
using PrettyBots.Tests.Environment.Interactions;
using PrettyBots.Tests.Environment.Parsers.Valid;
using PrettyBots.Tests.Environment.Responses;
using PrettyBots.Tests.Environment.Validators.Configs;
using PrettyBots.Tests.Environment.Validators.Valid;

namespace PrettyBots.Tests.Loading;

[Order(3)]
[TestFixture]
[TestOf(typeof(EntitiesLoader))]
[TestOf(typeof(LoadedEntitiesRegistry))]
public class InteractionTests : BaseLoadingTests
{
    [Test]
    [Order(1)]
    public void TestAfterParsersLoading_NoStrict()
    {
        GenericLoadingResult<IInteraction> loadingResult =
            InteractionService.Loader.LoadInteraction(InteractionDeclarations.ValidAfterParsersLoadingInteraction);
        Assert.That(loadingResult.Loaded, Is.False);

        InteractionService.Loader.LoadResponseParsers(EnvironmentAssembly);
        
        loadingResult = InteractionService.Loader.LoadInteraction(InteractionDeclarations.ValidAfterParsersLoadingInteraction);
        Assert.Multiple(() =>
        {
            Assert.That(loadingResult.Loaded, Is.True);
            Assert.That(loadingResult.Entity!.AvailableResponses[0]
                                     .ResponseParserType!.IsEquivalentTo(typeof(ValidDefaultTestResponseParser)));
        });
    }

    [Test]
    [Order(2)]
    public void TestAfterValidatorsLoading_NoStrict()
    {
        GenericLoadingResult<IInteraction> loadingResult =
            InteractionService.Loader.LoadInteraction(InteractionDeclarations.ValidAfterValidatorLoadingInteraction);
        Assert.That(loadingResult.Loaded, Is.False);

        InteractionService.Loader.LoadResponseParsers(EnvironmentAssembly);
        InteractionService.Loader.LoadResponseValidators(EnvironmentAssembly);
        
        loadingResult = InteractionService.Loader.LoadInteraction(InteractionDeclarations.ValidAfterValidatorLoadingInteraction);
        Assert.That(loadingResult.Loaded, Is.True);

        IResponseModel firstResponse  = loadingResult.Entity!.AvailableResponses[0];
        IResponseModel secondResponse = loadingResult.Entity!.AvailableResponses[1];
        IResponseModel thirdResponse  = loadingResult.Entity!.AvailableResponses[2];
        
        if (firstResponse is not IValidatableResponseModel<AbstractResponseImpl> vFirstResponse) {
            Assert.Fail("First response should be of type IValidatableResponseModel<TestResponse>");
            return;
        }
        
        if (secondResponse is not IValidatableResponseModel<TextResponse> vSecondResponse) {
            Assert.Fail("Second response should be of type IValidatableResponseModel<TextResponse>");
            return;
        }
        
        Assert.That(vFirstResponse.Config, Is.Not.Null);
        Assert.That(vFirstResponse.ResponseValidator, Is.Not.Null);
        Assert.That(vFirstResponse.ResponseValidatorType!
                                  .IsEquivalentTo(typeof(ValidAcceptMultipleValidator)));
        Assert.That(vFirstResponse.ResponseValidator.GetType()
                                  .IsEquivalentTo(vFirstResponse.ResponseValidatorType));

        AbstractConfigImpl firstResponseConfig = 
            (AbstractConfigImpl)vFirstResponse.ResponseValidatorConfig!;
        
        Assert.That(firstResponseConfig.TestParameter, Is.EqualTo(InteractionDeclarations.V2_TEST1_CONFIG_PARAMETER));
        
        Assert.That(vSecondResponse.ResponseValidator, Is.Not.Null);
        Assert.That(vSecondResponse.Config!.GetType().IsEquivalentTo(typeof(GeneralConfig)));
        Assert.That(vSecondResponse.ResponseValidator.GetType()
                                   .IsEquivalentTo(typeof(ValidAcceptAnyValidator)));
        
        Assert.That(thirdResponse is not IValidatableResponseModel);
    }

    [Test]
    [Order(3)]
    public void TestInvalidLoading_NoStrict()
    {
        InteractionService.Loader.LoadResponseParsers(EnvironmentAssembly);
        InteractionService.Loader.LoadResponseValidators(EnvironmentAssembly);
        
        InteractionService.Loader.LoadInteraction(InteractionDeclarations.ValidAfterParsersLoadingInteraction);
        foreach (IInteraction interaction in InteractionDeclarations.InvalidInteractions) {
            GenericLoadingResult<IInteraction> loadingResult = InteractionService.Loader.LoadInteraction(interaction);
            Assert.That(loadingResult.Loaded, Is.False);
        }
    }
}