using PrettyBots.Attributes.Responses;
using PrettyBots.Environment;
using PrettyBots.Environment.Responses;
using PrettyBots.Interactions.InteractionHandlers;
using PrettyBots.Interactions.Model.Context;
using PrettyBots.Interactions.Validators;
using PrettyBots.Interactions.Validators.Configs;
using PrettyBots.Tests.Environment.Interactions;
using PrettyBots.Tests.Environment.Messages;
using PrettyBots.Tests.Environment.Responses;
using PrettyBots.Tests.Environment.Validators.Valid;

namespace PrettyBots.Tests.Environment.InteractionModules;

public class AttributedInteractionModule : InteractionModuleBase
{
    public const string VA1_RES1_KEY = nameof(VA1_RES1_KEY);
    public const string VA1_RES2_KEY = nameof(VA1_RES2_KEY);
    public const string VA1_RES3_KEY = nameof(VA1_RES3_KEY);
    
    [LocalInteractionHandler(TestInteraction.VA1)]
    [RichTextResponse(VA1_RES1_KEY, typeof(RichTextResponseValidator), MinValue = 15, MaxValue = 255)]
    [BasicResponse(VA1_RES2_KEY, typeof(ImageResponse))]
    [ValidatableResponse(VA1_RES3_KEY, typeof(AdditionalResponse), typeof(ValidAcceptMultipleValidator))]
    public async Task ValidHandler1(IInteractionContext<TestMessage, IUserResponse> context)
    {
        
    }
}