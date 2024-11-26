using PrettyBots.Environment;

namespace PrettyBots.Interactions.Abstraction.Model.Context.Errors;

public class IncorrectUserMessageErrorContext : IIncorrectUserMessageErrorContext
{
    public IInteractionService            InteractionService { get; }
    public IInteraction                   TargetInteraction  { get; }
    public IUserMessage                       OriginalMessage    { get; }
    public IncorrectUserMessageErrorType  ErrorType          { get; }
    public List<ResponseValidationResult> ValidationResults  { get; }

    private IncorrectUserMessageErrorContext(IInteractionService interactionService, IInteraction targetInteraction, 
        IUserMessage originalMessage, IncorrectUserMessageErrorType errorType, 
        List<ResponseValidationResult> validationResults)
    {
        InteractionService = interactionService;
        TargetInteraction  = targetInteraction;
        OriginalMessage    = originalMessage;
        ErrorType          = errorType;
        ValidationResults  = validationResults;
    }

    public static IncorrectUserMessageErrorContext FromNoSuccessfulValidator(IInteractionService interactionService, 
        IInteraction targetInteraction, IUserMessage originalMessage,
        List<ResponseValidationResult> validationResults)
    {
        return new IncorrectUserMessageErrorContext(interactionService, targetInteraction, 
            originalMessage, IncorrectUserMessageErrorType.NoSuccessfulValidationResult, validationResults);
    }
}