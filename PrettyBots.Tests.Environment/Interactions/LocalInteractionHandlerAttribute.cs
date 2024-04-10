using PrettyBots.Interactions.Attributes;
using PrettyBots.Model.Descriptors.Config;

namespace PrettyBots.Tests.Environment.Interactions;

public class LocalInteractionHandlerAttribute : InteractionHandlerAttribute
{
    public LocalInteractionHandlerAttribute(TestInteraction interaction, HandlerRunMode runMode = HandlerRunMode.Default) 
        : base(Convert.ToUInt32(interaction), runMode)
    {
    }
}