using PrettyBots.Environment;
using PrettyBots.Interactions.Model.Context;
using PrettyBots.Model.Descriptors.Config;

namespace PrettyBots.Interactions.Attributes;

/// <summary>
/// Marks the method as an interaction handler.
/// When user responds to an interaction with the same id
/// as provided, this method will be invoked.
/// </summary>
/// <remarks>
/// Interaction handler method should be a non-abstract public method.
/// It can be async, in which case it should return <see cref="Task"/>.
/// The first necessary parameter of the handler is the object of type
/// <see cref="IInteractionContext{TMessage,TResponse}"/>, TResponse param should be
/// equal to <see cref="IUserResponse"/>, or to the specific response type
/// if it is only one available response for the interaction.
/// The second optional parameter is the object of type <see cref="CancellationToken"/>
/// that represents a token that will be used to cancel the handler task. 
/// </remarks>
[AttributeUsage(AttributeTargets.Method)]
public class InteractionHandlerAttribute : Attribute
{
    public uint InteractionId;
    public HandlerRunMode RunMode; 
    
    public InteractionHandlerAttribute(uint interactionId, 
        HandlerRunMode runMode = HandlerRunMode.Default)
    {
        InteractionId = interactionId;
        RunMode  = runMode;
    }
}