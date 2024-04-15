using System.Reflection;

using PrettyBots.Environment;
using PrettyBots.Interactions.Abstraction.Model.Context;
using PrettyBots.Interactions.Abstraction.Model.Delegates;

namespace PrettyBots.Interactions.Abstraction.Model.Descriptors;

public class HandlerExecutionContext<TMessage, TResponse> : HandlerExecutionContext
    where TMessage : class, IUserMessage
    where TResponse : class, IUserResponse
{
    public SyncInteractionHandler<TMessage, TResponse>? SyncHandler { private get; set; }
    public AsyncCancellableInteractionHandler<TMessage, TResponse>? AsyncCancellableHandler { private get; set; }
    public AsyncNonCancellableInteractionHandler<TMessage, TResponse>? AsyncNonCancellableHandler { private get; set; }

    public HandlerExecutionContext(IInteractionModule moduleInstance, MethodInfo methodInfo,
        bool isAsync = false, bool isCancellable = false)
    {
        IsAsync       = isAsync;
        IsCancellable = isCancellable;

        if (!isAsync) {
            GetType().GetProperty(nameof(SyncHandler))!.SetValue(this,
                Delegate.CreateDelegate(typeof(SyncInteractionHandler<TMessage, TResponse>),
                    moduleInstance, methodInfo));
            return;
        }
        
        if (isCancellable) {
            GetType().GetProperty(nameof(AsyncCancellableHandler))!.SetValue(this,
                Delegate.CreateDelegate(typeof(AsyncCancellableInteractionHandler<TMessage, TResponse>),
                    moduleInstance, methodInfo));
        } else {
            GetType().GetProperty(nameof(AsyncNonCancellableHandler))!.SetValue(this, 
                Delegate.CreateDelegate(typeof(AsyncNonCancellableInteractionHandler<TMessage, TResponse>),
                    moduleInstance, methodInfo));
        }
    }

    public override async Task Execute(IInteractionContext<IUserMessage, IUserResponse> context, 
        CancellationToken token = default)
    {
        IInteractionContext<TMessage, TResponse> typedContext = 
            (IInteractionContext<TMessage, TResponse>)context;
        
        if (AsyncCancellableHandler is not null) {
            await AsyncCancellableHandler.Invoke(typedContext, token).ConfigureAwait(false);
            return;
        }

        if (AsyncNonCancellableHandler is not null) {
            await AsyncNonCancellableHandler.Invoke(typedContext).ConfigureAwait(false);
            return;
        }

        SyncHandler!.Invoke(typedContext);
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
    
    public abstract Task Execute(IInteractionContext<IUserMessage, IUserResponse> context, 
        CancellationToken token = default);
}