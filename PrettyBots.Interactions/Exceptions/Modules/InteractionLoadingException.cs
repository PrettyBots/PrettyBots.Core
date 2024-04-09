using PrettyBots.Model;

namespace PrettyBots.Interactions.Exceptions.Modules;

public class InteractionLoadingException : Exception
{
    public IInteraction Interaction { get; }

    public InteractionLoadingException(IInteraction interaction, string message)
        : base($"Loading interaction with id {interaction.Id} failed: {message}")
    {
        Interaction = interaction;
    }
}