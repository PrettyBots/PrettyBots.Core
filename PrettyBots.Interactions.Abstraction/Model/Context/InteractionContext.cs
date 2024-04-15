using PrettyBots.Environment;

namespace PrettyBots.Interactions.Abstraction.Model.Context;

/// <inheritdoc />
public class InteractionContext<TMessage, TResponse> : IInteractionContext<TMessage, TResponse>
    where TMessage : IUserMessage
    where TResponse : IUserResponse
{
    /// <inheritdoc />
    public IInteractionService InteractionService { get; }
    
    /// <inheritdoc />
    public IInteraction TargetInteraction { get; }
    
    /// <inheritdoc />
    public string ResponseKey { get; }

    /// <inheritdoc />
    public TResponse Response { get; }

    public TMessage OriginalMessage { get; }

    public InteractionContext(IInteractionService interactionService, 
        IInteraction targetInteraction, string responseKey,
        TResponse response, TMessage originalMessage)
    {
        Response             = response;
        ResponseKey          = responseKey;
        OriginalMessage      = originalMessage;
        TargetInteraction    = targetInteraction;
        InteractionService   = interactionService;
    }
}