using PrettyBots.Environment;

namespace PrettyBots.Interactions.Abstraction.Model.Context;

/// <summary>
/// Contains data of the context
/// in which user has responded to an interaction.
/// </summary>
public interface IInteractionContext<out TMessage, out TResponse>
    where TMessage  : IUserMessage
    where TResponse : IUserResponse
{
    /// <summary>
    /// Service that handled the interaction.
    /// </summary>
    IInteractionService InteractionService { get; }

    /// <summary>
    /// Interaction to which the user has responded.
    /// </summary>
    IInteraction TargetInteraction { get; }

    /// <summary>
    /// Key of the valid interaction response that had been configured in order to
    /// accept the <see cref="Response"/>. 
    /// </summary>
    string ResponseKey { get; }

    /// <summary>
    /// Contains the user's response.
    /// </summary>
    TResponse Response { get; }
    
    /// <summary>
    /// Contains original message that was handled and parsed into the <see cref="Response"/>.
    /// </summary>
    TMessage OriginalMessage { get; }
}