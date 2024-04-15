using PrettyBots.Environment;
using PrettyBots.Interactions.Abstraction.Model.Context;

namespace PrettyBots.Interactions.Abstraction.Model.Delegates;

public delegate Task AsyncCancellableInteractionHandler<in TMessage, in TResponse>(
    IInteractionContext<TMessage, TResponse> context, 
    CancellationToken token = default)
    where TMessage  : class, IUserMessage
    where TResponse : class, IUserResponse;
