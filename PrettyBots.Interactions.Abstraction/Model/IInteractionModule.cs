namespace PrettyBots.Interactions.Abstraction.Model;

public interface IInteractionModule
{
    /// <summary>
    /// This method serves to declare interactions that will be parsed automatically
    /// when this module is loaded into the interactions service.
    /// </summary>
    public IEnumerable<IInteraction> DeclareInteractions();
}