using PrettyBots.Environment;
using PrettyBots.Storages.Abstraction.Model;

namespace PrettyBots.Interactions.Abstraction.Model.Context;

/// <summary>
/// Contains data of the context
/// in which user has responded to an interaction.
/// </summary>
/// TODO: Add abstract response support if response was are instantiable from this response type
public interface IInteractionContext<out TMessage, out TUser, out TResponse>
    where TMessage : IUserMessage
    where TUser : IUser
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

    TUser User { get; }

    /// <summary>
    /// Key of the valid interaction response that had been configured in order to
    /// accept the <see cref="Response"/>. 
    /// </summary>
    string ResponseKey { get; }

    string? InteractionDataString { get; set; }

    bool DataChanged { get; set; }

    TData? GetInteractionData<TData>() 
        where TData: class;

    void SetInteractionData(object? data); 

    /// <summary>
    /// Contains the user's response.
    /// </summary>
    TResponse Response { get; }
    
    /// <summary>
    /// Contains original message that was handled and parsed into the <see cref="Response"/>.
    /// </summary>
    TMessage OriginalMessage { get; }
}