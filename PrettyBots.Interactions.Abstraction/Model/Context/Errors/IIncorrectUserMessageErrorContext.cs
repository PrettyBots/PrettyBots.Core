using PrettyBots.Environment;

namespace PrettyBots.Interactions.Abstraction.Model.Context.Errors;

public interface IIncorrectUserMessageErrorContext     
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
    /// Contains original message that was handled and parsed into the <see cref="Response"/>.
    /// </summary>
    IUserMessage OriginalMessage { get; }
    
    IncorrectUserMessageErrorType ErrorType { get; }
    
    List<ResponseValidationResult> ValidationResults { get; }
}