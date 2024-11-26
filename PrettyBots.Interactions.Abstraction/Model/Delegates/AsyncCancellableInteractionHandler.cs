using PrettyBots.Environment;
using PrettyBots.Interactions.Abstraction.Model.Context;
using PrettyBots.Storages.Abstraction.Model;

namespace PrettyBots.Interactions.Abstraction.Model.Delegates;

public delegate Task AsyncCancellableInteractionHandler<in TMessage, in TUser, in TResponse>(
    IInteractionContext<TMessage, TUser, TResponse> context, 
    CancellationToken token = default)
    where TMessage  : class, IUserMessage
    where TUser     : class, IUser
    where TResponse : class, IUserResponse;
