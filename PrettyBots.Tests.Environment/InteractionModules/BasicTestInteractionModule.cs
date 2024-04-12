using JetBrains.Annotations;

using PrettyBots.Environment;
using PrettyBots.Environment.Responses;
using PrettyBots.Interactions.Attributes;
using PrettyBots.Interactions.Builders;
using PrettyBots.Interactions.Model.Context;
using PrettyBots.Model;
using PrettyBots.Model.Descriptors.Config;
using PrettyBots.Tests.Environment.Interactions;
using PrettyBots.Tests.Environment.Messages;
using PrettyBots.Tests.Environment.Services;

namespace PrettyBots.Tests.Environment.InteractionModules;

/// <summary>
/// Tests basic reflection utilities against the valid
/// and invalid response handlers.
/// </summary>
public class BasicTestInteractionModule : IInteractionModule
{
    public const string I1_KEY  = "test_text_1";
    public const string I2_KEY  = "test_image_2_1";
    public const string I2_KEY2 = "test_text_2_2";
    public const string I3_KEY  = "test_text_3";

    public ITestService? Service { get; }

    [UsedImplicitly]
    public BasicTestInteractionModule()
    {
        
    }
    
    [UsedImplicitly]
    public BasicTestInteractionModule(ITestService service)
    {
        Service = service;
    }
    
    public IEnumerable<IInteraction> DeclareInteractions()
    {
        return new[] {
            InteractionBuilder
                .WithId((uint)TestInteraction.BMI1)
                .WithResponse(TestResponseModelBuilder<TextResponse>
                    .WithKey(I1_KEY))
                .Build(),
            InteractionBuilder
                .WithId((uint)TestInteraction.BMI2)
                .WithResponse(TestResponseModelBuilder<ImageResponse>
                    .WithKey(I2_KEY))
                .WithResponse(TestResponseModelBuilder<TextResponse>
                    .WithKey(I2_KEY2))
                .Build(),
            InteractionBuilder
                .WithId((uint)TestInteraction.BMI3)
                .WithResponse(TestResponseModelBuilder<ImageResponse>
                    .WithKey(I3_KEY))
                .Build(),
        };
    }

    /// <summary>
    /// Tests valid handler for not declared interaction.
    /// </summary>
    [InteractionHandler((uint)TestInteraction.NotDefinedI, HandlerRunMode.RunSync)]
    public void NotDefinedInteractionHandler(IInteractionContext<TestMessage, TextResponse> context,
        CancellationToken token = default)
    {
        
    }
    
    /// <summary>
    /// Tests handler with invalid return type.
    /// </summary>
    [InteractionHandler((uint)TestInteraction.BMI1, HandlerRunMode.RunSync)]
    public string InvalidReturnTypeHandler(IInteractionContext<TestMessage, TextResponse> context,
        CancellationToken token = default)
    {
        return "test";
    }
    
    /// <summary>
    /// Tests handler with incorrect definition.
    /// </summary>
    [InteractionHandler((uint)TestInteraction.BMI1, HandlerRunMode.RunSync)]
    public static void InvalidWrongDefinitionHandler(IInteractionContext<TestMessage, TextResponse> context,
        CancellationToken token = default)
    {
        
    }

    /// <summary>
    /// Tests handler with too many parameters.
    /// </summary>
    [InteractionHandler((uint)TestInteraction.BMI1, HandlerRunMode.RunSync)]
    public void InvalidArgumentsTypeHandler1(IInteractionContext<TestMessage, TextResponse> context,
        CancellationToken token = default, string s = "")
    {
        
    }
    
    /// <summary>
    /// Tests handler with invalid second parameter.
    /// </summary>
    [InteractionHandler((uint)TestInteraction.BMI1, HandlerRunMode.RunSync)]
    public void InvalidArgumentsTypeHandler2(IInteractionContext<TestMessage, TextResponse> context,
        string token = "")
    {
        
    }
    
    /// <summary>
    /// Tests handler with invalid first parameter (not context).
    /// </summary>
    [InteractionHandler((uint)TestInteraction.BMI1, HandlerRunMode.RunSync)]
    public void InvalidArgumentsTypeHandler3(List<TextResponse> context,
        CancellationToken token = default)
    {
        
    }
    
    /// <summary>
    /// Tests handler with invalid first parameter (strong type doesn't match with interaction).
    /// </summary>
    [InteractionHandler((uint)TestInteraction.BMI1, HandlerRunMode.RunSync)]
    public void InvalidArgumentsTypeHandler4(IInteractionContext<TestMessage, ImageResponse> context,
        CancellationToken token = default)
    {
        
    }
    
    /// <summary>
    /// Tests handler with invalid first parameter (strong type used on multiple-type response).
    /// </summary>
    [InteractionHandler((uint)TestInteraction.BMI2, HandlerRunMode.RunSync)]
    public void InvalidArgumentsTypeHandler5(IInteractionContext<TestMessage, ImageResponse> context,
        CancellationToken token = default)
    {
        
    }
    
    /// <summary>
    /// Tests invalid sync handler with context set to incorrect message type.
    /// </summary>
    [InteractionHandler((int)TestInteraction.BMI1, HandlerRunMode.RunSync)]
    public void InvalidArgumentsTypeHandler6(IInteractionContext<UnusedMessage, TextResponse> context,
        CancellationToken token = default)
    {
        
    }
    
    /// <summary>
    /// Tests valid sync handler (type is strong).
    /// </summary>
    [InteractionHandler((int)TestInteraction.BMI1, HandlerRunMode.RunSync)]
    public void ValidHandler1(IInteractionContext<TestMessage, TextResponse> context,
        CancellationToken token = default)
    {
        
    }
    
    /// <summary>
    /// Tests valid sync handler, but invalid because it is defined to the
    /// interaction that already has loaded handler. 
    /// </summary>
    [InteractionHandler((int)TestInteraction.BMI1, HandlerRunMode.RunSync)]
    public void InvalidDuplicateHandler(IInteractionContext<TestMessage, TextResponse> context,
        CancellationToken token = default)
    {
        
    }
    
    /// <summary>
    /// Tests valid sync handler (type is dynamic).
    /// </summary>
    [InteractionHandler((int)TestInteraction.BMI2, HandlerRunMode.RunAsync)]
    public void ValidHandler2(IInteractionContext<TestMessage, IUserResponse> context,
        CancellationToken token = default)
    {
        
    }
    
    /// <summary>
    /// Tests valid async not cancellable handler (type is strong).
    /// </summary>
    [InteractionHandler((int)TestInteraction.BMI3)]
    public async Task ValidHandler3(IInteractionContext<TestMessage, ImageResponse> context)
    {
        
    }
}