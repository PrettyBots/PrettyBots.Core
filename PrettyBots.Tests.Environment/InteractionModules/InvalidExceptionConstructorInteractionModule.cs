using PrettyBots.Interactions.InteractionHandlers;

namespace PrettyBots.Tests.Environment.InteractionModules;

public class InvalidExceptionConstructorInteractionModule : InteractionModuleBase
{
    public InvalidExceptionConstructorInteractionModule()
    {
        throw new IgnorableException();
    }
}