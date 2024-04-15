using PrettyBots.Environment;
using PrettyBots.Interactions.Abstraction.Model.Context;

namespace PrettyBots.Interactions.Abstraction.Model.Delegates;

public delegate void SyncInteractionHandler<in TMessage, in TResponse>(
    IInteractionContext<TMessage, TResponse> context)
    where TMessage  : class, IUserMessage
    where TResponse : class, IUserResponse;