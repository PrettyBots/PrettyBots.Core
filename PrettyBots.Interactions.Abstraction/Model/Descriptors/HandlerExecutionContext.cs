using System.Reflection;

using PrettyBots.Environment;
using PrettyBots.Interactions.Abstraction.Model.Context;
using PrettyBots.Interactions.Abstraction.Model.Delegates;
using PrettyBots.Storages.Abstraction.Model;

namespace PrettyBots.Interactions.Abstraction.Model.Descriptors;

public class HandlerExecutionContext<TMessage, TUser, TResponse> : HandlerExecutionContext
    where TMessage : class, IUserMessage
    where TUser     : class, IUser
    where TResponse : class, IUserResponse
{
    public SyncInteractionHandler<TMessage, TUser, TResponse>? SyncHandler { private get; set; }
    public AsyncCancellableInteractionHandler<TMessage, TUser, TResponse>? AsyncCancellableHandler { private get; set; }
    public AsyncNonCancellableInteractionHandler<TMessage, TUser, TResponse>? AsyncNonCancellableHandler { private get; set; }

    public HandlerExecutionContext(IInteractionModule moduleInstance, MethodInfo methodInfo,
        bool isAsync = false, bool isCancellable = false)
    {
        IsAsync       = isAsync;
        IsCancellable = isCancellable;

        if (!isAsync) {
            GetType().GetProperty(nameof(SyncHandler))!.SetValue(this,
                Delegate.CreateDelegate(typeof(SyncInteractionHandler<TMessage, TUser, TResponse>),
                    moduleInstance, methodInfo));
            return;
        }
        
        if (isCancellable) {
            GetType().GetProperty(nameof(AsyncCancellableHandler))!.SetValue(this,
                Delegate.CreateDelegate(typeof(AsyncCancellableInteractionHandler<TMessage, TUser, TResponse>),
                    moduleInstance, methodInfo));
        } else {
            GetType().GetProperty(nameof(AsyncNonCancellableHandler))!.SetValue(this, 
                Delegate.CreateDelegate(typeof(AsyncNonCancellableInteractionHandler<TMessage, TUser, TResponse>),
                    moduleInstance, methodInfo));
        }
    }

    public override async Task Execute(IInteractionContext<IUserMessage, IUser, IUserResponse> context, 
        CancellationToken token = default)
    {
        IInteractionContext<TMessage, TUser, TResponse> typedContext = 
            new InteractionContext<TMessage, TUser, TResponse>(
                context.InteractionService, 
                context.TargetInteraction,
                context.ResponseKey, 
                (TResponse)context.Response, 
                (TMessage)context.OriginalMessage,
                (TUser)context.User
            ); 

        
        if (AsyncCancellableHandler is not null) {
            await AsyncCancellableHandler.Invoke(typedContext, token).ConfigureAwait(false);
            context.DataChanged = typedContext.DataChanged;
            return;
        }

        if (AsyncNonCancellableHandler is not null) {
            await AsyncNonCancellableHandler.Invoke(typedContext).ConfigureAwait(false);
            context.DataChanged = typedContext.DataChanged;
            return;
        }

        SyncHandler!.Invoke(typedContext);
        context.DataChanged = typedContext.DataChanged;
    }
}

/// <summary>
/// Describes how exactly the handler should be called,
/// providing non-reflection methods to call it after
/// the loading process.
/// </summary>
public abstract class HandlerExecutionContext
{
    /// <summary>
    /// Specifies whether this handler has a return type of <see cref="Task"/>.
    /// </summary>
    public bool IsAsync { get; protected set; }

    /// <summary>
    /// Specifies whether this handler has the second parameter of <see cref="CancellationToken"/>. 
    /// </summary>
    public bool IsCancellable { get; protected set; }
    
    public abstract Task Execute(IInteractionContext<IUserMessage, IUser, IUserResponse> context, 
        CancellationToken token = default);
}