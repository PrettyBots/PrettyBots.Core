using Microsoft.Extensions.DependencyInjection;

using PrettyBots.Environment;
using PrettyBots.Environment.Responses;
using PrettyBots.Environment.Responses.TextBased;
using PrettyBots.Interactions.Abstraction.Model.Context;
using PrettyBots.Interactions.Abstraction.Model.Descriptors;
using PrettyBots.Interactions.Abstraction.Model.Descriptors.Config;
using PrettyBots.Interactions.Abstraction.Model.Descriptors.Loading;
using PrettyBots.Interactions.Abstraction.Model.Descriptors.Loading.Abstraction;
using PrettyBots.Interactions.Abstraction.Model.Responses;
using PrettyBots.Interactions.Exceptions.Modules;
using PrettyBots.Interactions.Services;
using PrettyBots.Interactions.Validators;
using PrettyBots.Interactions.Validators.Text;
using PrettyBots.Interactions.Validators.Text.Configs;
using PrettyBots.Tests.Environment.InteractionModules;
using PrettyBots.Tests.Environment.Interactions;
using PrettyBots.Tests.Environment.Parsers.Valid;
using PrettyBots.Tests.Environment.Responses;
using PrettyBots.Tests.Environment.Services;
using PrettyBots.Tests.Environment.Validators.Valid;

namespace PrettyBots.Tests.Loading;

[Order(4)]
[TestFixture]
[TestOf(typeof(EntitiesLoader))]
public class ModuleTests : BaseLoadingTests
{
    private static readonly string[] _validInteractionModules = {
        nameof(BasicTestInteractionModule),
        nameof(AttributedInteractionModule),
    };
    
    private static readonly string[] _invalidInteractionModules = {
        nameof(EmptyInteractionModule),
        nameof(InvalidDefinitionInteractionModule),
        nameof(InvalidConstructorInteractionModule),
        nameof(InvalidExceptionConstructorInteractionModule),
    };
    
    private static readonly string[] _basicModuleInvalidHandlerNames = {
        nameof(BasicTestInteractionModule.NotDefinedInteractionHandler),
        nameof(BasicTestInteractionModule.InvalidWrongDefinitionHandler),
        nameof(BasicTestInteractionModule.InvalidReturnTypeHandler),
        nameof(BasicTestInteractionModule.InvalidArgumentsTypeHandler1),
        nameof(BasicTestInteractionModule.InvalidArgumentsTypeHandler2),
        nameof(BasicTestInteractionModule.InvalidArgumentsTypeHandler3),
        nameof(BasicTestInteractionModule.InvalidArgumentsTypeHandler4),
        nameof(BasicTestInteractionModule.InvalidArgumentsTypeHandler5),
        nameof(BasicTestInteractionModule.InvalidArgumentsTypeHandler6),
        nameof(BasicTestInteractionModule.InvalidArgumentsTypeHandler7),
        nameof(BasicTestInteractionModule.InvalidDuplicateHandler),
    };
    
    private static readonly string[] _basicModuleValidHandlerNames = {
        nameof(BasicTestInteractionModule.ValidHandler1),
        nameof(BasicTestInteractionModule.ValidHandler2),
        nameof(BasicTestInteractionModule.ValidHandler3),
    };

    [Test]
    public void TestModulesLoading_NoSP_NoStrict()
    {
        InteractionService.Config.StrictLoadingModeEnabled = false;

        InteractionService.Loader.LoadResponseParsers(EnvironmentAssembly);
        InteractionService.Loader.LoadResponseValidators(EnvironmentAssembly);
        
        MultipleLoadingResult<ModuleLoadingResult> loadingResult = 
            InteractionService.Loader.LoadInteractionModules(
                EnvironmentAssembly);

        TestLoadingResults(loadingResult);

        ModuleLoadingResult duplicateLoadingResult =
            InteractionService.Loader.LoadInteractionModule<BasicTestInteractionModule>();
        Assert.That(duplicateLoadingResult.Loaded, Is.False);
    }

    [Test]
    public void TestModulesLoading_SP_NoStrict()
    {
        InteractionService.Config.StrictLoadingModeEnabled = false;

        IServiceProvider provider = new ServiceCollection()
            .AddSingleton<ITestService, TestService>()
            .AddSingleton<BasicTestInteractionModule>()
            .BuildServiceProvider();

        provider.GetRequiredService<ITestService>().Test = ITestService.TEST_VARIABLE_VALUE;
        
        InteractionService.Loader.LoadResponseParsers(EnvironmentAssembly, provider);
        InteractionService.Loader.LoadResponseValidators(EnvironmentAssembly, provider);
        
        MultipleLoadingResult<ModuleLoadingResult> loadingResult = 
            InteractionService.Loader.LoadInteractionModules(
                EnvironmentAssembly, provider);

        ModuleLoadingResult        basicTestModule = TestLoadingResults(loadingResult);
        BasicTestInteractionModule moduleInstance  = (BasicTestInteractionModule)basicTestModule.Info!.Instance;
        
        Assert.That(moduleInstance.Service!.Test, Is.EqualTo(ITestService.TEST_VARIABLE_VALUE));
        TestLoadingResults(loadingResult);
    }

    [Test]
    public void TestModulesLoading_NoSP_Strict()
    {
        InteractionService.Config.StrictLoadingModeEnabled = false;
        
        InteractionService.Loader.LoadResponseParsers(EnvironmentAssembly);
        InteractionService.Loader.LoadResponseValidators(EnvironmentAssembly);
        
        InteractionService.Config.StrictLoadingModeEnabled = true;
        Assert.Throws<HandlerLoadingException>( () => {
            InteractionService.Loader.LoadInteractionModules(EnvironmentAssembly);
        });
    }

    private ModuleLoadingResult TestLoadingResults(MultipleLoadingResult<ModuleLoadingResult> loadingResult)
    {
        Assert.That(loadingResult.Loaded, Is.True);
        Assert.That(loadingResult.Loaded, Is.True);
        Assert.That(loadingResult.Entities, Is.Not.Null);
        Assert.That(loadingResult.Entities, Has.Count.EqualTo(6));
        
        CollectionAssert.AreEquivalent(_validInteractionModules, loadingResult.Entities!
            .Where(e => e.Loaded)
            .Select(e => e.EntityName));
        
         CollectionAssert.AreEquivalent(_invalidInteractionModules, loadingResult.Entities!
            .Where(e => !e.Loaded)
            .Select(e => e.EntityName));
        
        ModuleLoadingResult basicTestModule = loadingResult.Entities!.First(e 
            => e.Info!.Type.IsEquivalentTo(typeof(BasicTestInteractionModule)));
        
        IEnumerable<string> failedEntityNames = basicTestModule.HandlerLoadingResults!
            .Where(r => !r.Loaded)
            .Select(r => r.EntityName)
            .ToArray();

        IEnumerable<InteractionHandlerInfo> successHandlerInfos = basicTestModule.HandlerLoadingResults!
            .Where(r => r.Loaded)
            .Select(r => r.Entity!).ToArray();
        
        IEnumerable<string> successEntityNames = successHandlerInfos
            .Select(i => i.Name)
            .ToArray();
        
        CollectionAssert.AreEquivalent(_basicModuleInvalidHandlerNames, failedEntityNames);
        CollectionAssert.AreEquivalent(_basicModuleValidHandlerNames, successEntityNames);
        
        InteractionHandlerInfo validHandler1 = successHandlerInfos
            .First(i => i.Name == nameof(BasicTestInteractionModule.ValidHandler1));
        InteractionHandlerInfo validHandler2 = successHandlerInfos
            .First(i => i.Name == nameof(BasicTestInteractionModule.ValidHandler2));
        InteractionHandlerInfo validHandler3 = successHandlerInfos
            .First(i => i.Name == nameof(BasicTestInteractionModule.ValidHandler3));
        
        Assert.That(validHandler1.RunMode, Is.EqualTo(HandlerRunMode.RunSync));
        Assert.That(validHandler1.ExecutionContext.IsAsync, Is.True);
        Assert.That(validHandler1.ExecutionContext.IsCancellable, Is.False);
        Assert.That(validHandler1.InteractionId, Is.EqualTo((uint)TestInteraction.BMI1));
        Assert.That(validHandler1.AcceptsSpecificContext, Is.True);
        Assert.That(validHandler1.SpecificContextResponseType!.IsEquivalentTo(typeof(TextResponse)));
        
        Assert.That(validHandler2.RunMode, Is.EqualTo(HandlerRunMode.RunAsync));
        Assert.That(validHandler2.ExecutionContext.IsAsync, Is.False);
        Assert.That(validHandler2.ExecutionContext.IsCancellable, Is.False);
        Assert.That(validHandler2.InteractionId, Is.EqualTo((uint)TestInteraction.BMI2));
        Assert.That(validHandler2.AcceptsSpecificContext, Is.False);
        Assert.That(validHandler2.SpecificContextResponseType, Is.Null);
        
        Assert.That(validHandler3.RunMode, Is.EqualTo(HandlerRunMode.Default));
        Assert.That(validHandler3.ExecutionContext.IsAsync, Is.True);
        Assert.That(validHandler3.ExecutionContext.IsCancellable, Is.True);
        Assert.That(validHandler3.InteractionId, Is.EqualTo((uint)TestInteraction.BMI3));
        Assert.That(validHandler3.AcceptsSpecificContext, Is.True);
        Assert.That(validHandler3.SpecificContextResponseType!.IsEquivalentTo(typeof(ImageResponse)));
        
        ModuleLoadingResult attributedModuleTests = loadingResult.Entities!.First(e 
            => e.Info!.Type.IsEquivalentTo(typeof(AttributedInteractionModule)));
        
        Assert.That(attributedModuleTests.HandlerLoadingResults, Has.Count.EqualTo(1));
        Assert.That(attributedModuleTests.InteractionLoadingResults, Has.Count.EqualTo(1));
        Assert.That(InteractionService.Registry.Interactions.ContainsKey((uint)TestInteraction.VA1));

        InteractionInfo va1 = InteractionService.Registry.Interactions[(uint)TestInteraction.VA1];
        Assert.That(va1.HandlerInfo, Is.Not.Null);
        Assert.That(va1.Interaction, Is.Not.Null);
        Assert.That(va1.Interaction.AvailableResponses, Has.Count.EqualTo(3));
        
        IValidatableResponseModel<TextResponse> res1 = 
            va1.Interaction.GetValidatableResponse<TextResponse>
                (AttributedInteractionModule.VA1_RES1_KEY)!;
        
        IResponseModel res2 = 
            va1.Interaction.GetResponse
                (AttributedInteractionModule.VA1_RES2_KEY)!;

        IValidatableResponseModel<AdditionalResponse> res3 = 
            va1.Interaction.GetValidatableResponse<AdditionalResponse>
                (AttributedInteractionModule.VA1_RES3_KEY)!;

        Assert.That(res1.ResponseParserType!.IsEquivalentTo(typeof(ValidTextParser)));
        Assert.That(res1.ResponseValidatorType!.IsEquivalentTo(typeof(RichTextResponseValidator)));
        Assert.That(res1.ResponseValidator!.GetType().IsEquivalentTo(res1.ResponseValidatorType));
        Assert.That(res1.ResponseValidatorConfig!.GetType().IsEquivalentTo(typeof(RichTextValidatorConfig)));

        RichTextValidatorConfig config = (RichTextValidatorConfig)res1.ResponseValidatorConfig;
        Assert.That(config.MinValue, Is.EqualTo(15U));
        Assert.That(config.MaxValue, Is.EqualTo(255U));
        
        Assert.That(res2.ResponseType.IsEquivalentTo(typeof(ImageResponse)));
        Assert.That(res2.ResponseParserType!.IsEquivalentTo(typeof(ValidImageParser)));
        
        Assert.That(res3.ResponseValidatorConfig, Is.Null);
        Assert.That(res3.ResponseType.IsEquivalentTo(typeof(AdditionalResponse)));
        Assert.That(res3.ResponseValidatorType!.IsEquivalentTo(typeof(ValidAcceptMultipleValidator)));
        
        return basicTestModule;
    }
}