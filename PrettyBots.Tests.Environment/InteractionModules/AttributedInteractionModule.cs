using PrettyBots.Attributes.Responses;
using PrettyBots.Attributes.Responses.Basic;
using PrettyBots.Environment;
using PrettyBots.Environment.Responses;
using PrettyBots.Environment.Responses.TextBased;
using PrettyBots.Interactions.Abstraction.Model.Context;
using PrettyBots.Interactions.InteractionHandlers;
using PrettyBots.Interactions.Validators;
using PrettyBots.Interactions.Validators.Text;
using PrettyBots.Interactions.Validators.Text.Configs;
using PrettyBots.Tests.Environment.Interactions;
using PrettyBots.Tests.Environment.Messages;
using PrettyBots.Tests.Environment.Responses;
using PrettyBots.Tests.Environment.Storage;
using PrettyBots.Tests.Environment.Validators.Valid;

namespace PrettyBots.Tests.Environment.InteractionModules;

public class AttributedInteractionModule : InteractionModuleBase
{
    public const string VA1_RES1_KEY = nameof(VA1_RES1_KEY);
    public const string VA1_RES2_KEY = nameof(VA1_RES2_KEY);
    public const string VA1_RES3_KEY = nameof(VA1_RES3_KEY);
    
    [LocalInteractionHandler(TestInteraction.VA1)]
    [RichTextResponse(VA1_RES1_KEY, typeof(TextResponse), MinValue = 15, MaxValue = 255)]
    [BasicResponse(VA1_RES2_KEY, typeof(ImageResponse))]
    [ValidatableResponse(VA1_RES3_KEY, typeof(AdditionalResponse), typeof(ValidAcceptMultipleValidator))]
    public async Task ValidHandler1(IInteractionContext<TestMessage, TestUser, IUserResponse> context)
    {
        
    }
}