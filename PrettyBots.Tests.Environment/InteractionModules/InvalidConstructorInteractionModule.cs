using JetBrains.Annotations;

using PrettyBots.Interactions.InteractionHandlers;

namespace PrettyBots.Tests.Environment.InteractionModules;

public class InvalidConstructorInteractionModule : InteractionModuleBase
{
    public InvalidConstructorInteractionModule([UsedImplicitly]string a)
    {
        throw new NotImplementedException();
    }
}