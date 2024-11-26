using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using PrettyBots.Environment;
using PrettyBots.Interactions.Abstraction.Model.Context;
using PrettyBots.Interactions.Abstraction.Model.Descriptors.Config;

namespace PrettyBots.Interactions.Abstraction.Model.Descriptors;

public class InteractionHandlerInfo
{
    public readonly uint InteractionId;
    
    /// <inheritdoc cref="HandlerRunMode"/>
    public readonly HandlerRunMode RunMode;

    public string Name => MethodInfo.Name; 
        
    /// <summary>
    /// Reflection info about the handler method.
    /// </summary>
    public readonly MethodInfo MethodInfo;
    
    
    /// <summary>
    /// Descriptor of the module that contains this handler.
    /// </summary>
    public readonly InteractionModuleInfo Module;

    /// <summary>
    /// Specifies whether the handler accepts the <see cref="IInteractionContext{TMessage,TResponse}"/>
    /// with TResponse set to a specific response type, in which case
    /// <see cref="SpecificContextResponseType"/> will contain the type of the response.
    /// </summary>
    [MemberNotNullWhen(true, nameof(SpecificContextResponseType))]
    public bool AcceptsSpecificContext { get; }
    public Type? SpecificContextResponseType { get; }
    
    /// <summary>
    /// Message type that implements <see cref="IUserMessage"/> and
    /// should be equal to the environment message type. 
    /// </summary>
    public Type ContextMessageType { get; }

    public Type UserType { get; }

    /// <inheritdoc cref="HandlerExecutionContext"/>
    public HandlerExecutionContext ExecutionContext { get; }


  
    public InteractionHandlerInfo(uint interactionId, HandlerRunMode runMode, 
        MethodInfo methodInfo, InteractionModuleInfo module, Type contextMessageType, Type userType, 
        bool acceptsSpecificContext = false, bool isAsync = false, bool isCancellable = false, 
        Type? specificContextResponseType = null)
    {
        Module                      = module;
        RunMode                     = runMode;
        UserType                    = userType;
        MethodInfo                  = methodInfo;
        InteractionId               = interactionId;
        ContextMessageType          = contextMessageType;
        AcceptsSpecificContext      = acceptsSpecificContext;
        SpecificContextResponseType = specificContextResponseType;
        
        if (!AcceptsSpecificContext) {
            ExecutionContext = (HandlerExecutionContext)Activator.CreateInstance(
                typeof(HandlerExecutionContext<,,>).MakeGenericType(contextMessageType, UserType,
                    typeof(IUserResponse)), module.Instance, methodInfo, isAsync, 
                isCancellable)!;
            return;
        }
        
        ExecutionContext = (HandlerExecutionContext)Activator.CreateInstance(
            typeof(HandlerExecutionContext<,,>).MakeGenericType(contextMessageType, UserType,
                SpecificContextResponseType), module.Instance, methodInfo, isAsync, 
            isCancellable)!;
    }
}