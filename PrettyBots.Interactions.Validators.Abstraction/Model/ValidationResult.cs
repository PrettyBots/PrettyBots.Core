using System.Diagnostics.CodeAnalysis;

namespace PrettyBots.Interactions.Validators.Abstraction.Model;

public class ValidationResult
{
    [MemberNotNullWhen(false, nameof(ErrorMessage))]
    [MemberNotNullWhen(false, nameof(ErrorType))]
    public bool Success { get; }
    public ValidatorErrorType? ErrorType { get; }
    public string? ErrorMessage { get; }
    
    private ValidationResult(bool success, ValidatorErrorType? errorType, string? errorMessage)
    {
        Success      = success;
        ErrorType    = errorType;
        ErrorMessage = errorMessage;
    }

    public static ValidationResult Ok()
    {
        return new ValidationResult(true, null, null);
    }

    public static ValidationResult Error(ValidatorErrorType errorType, string error)
    {
        return new ValidationResult(false, errorType, error);
    }

}