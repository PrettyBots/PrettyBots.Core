using PrettyBots.Model;

namespace PrettyBots.Interactions.InteractionHandlers;

public class InteractionModuleBase : IInteractionModule
{
    public virtual IEnumerable<IInteraction> DeclareInteractions() => 
        Enumerable.Empty<IInteraction>();
}