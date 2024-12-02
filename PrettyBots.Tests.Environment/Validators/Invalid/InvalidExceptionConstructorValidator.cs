using PrettyBots.Interactions.Validators.Abstraction;
using PrettyBots.Interactions.Validators.Abstraction.Model;
using PrettyBots.Tests.Environment.Responses;

namespace PrettyBots.Tests.Environment.Validators.Invalid;

public class InvalidExceptionConstructorValidator : ResponseValidator<AdditionalResponse>
{
    public InvalidExceptionConstructorValidator()
    {
        throw new IgnorableException();
    }
    
    protected override ValueTask<ValidationResult> ValidateAsync(AdditionalResponse response, IValidatorConfig config) { throw new NotImplementedException(); }
}