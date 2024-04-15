using PrettyBots.Interactions.Abstraction.Model.Descriptors.Config;
using PrettyBots.Interactions.Attributes;

namespace PrettyBots.Tests.Environment.Interactions;

public class LocalInteractionHandlerAttribute : InteractionHandlerAttribute
{
    public LocalInteractionHandlerAttribute(TestInteraction interaction, HandlerRunMode runMode = HandlerRunMode.Default) 
        : base(Convert.ToUInt32(interaction), runMode)
    {
    }
}