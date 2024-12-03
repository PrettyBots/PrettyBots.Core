using PrettyBots.Tests.Environment.Responses;
using PrettyBots.Validators.Abstraction;
using PrettyBots.Validators.Abstraction.Model;

namespace PrettyBots.Tests.Environment.Validators.Invalid;

public class InvalidExceptionConstructorValidator : ResponseValidator<AdditionalResponse>
{
    public InvalidExceptionConstructorValidator()
    {
        throw new IgnorableException();
    }
    
    protected override ValueTask<ValidationResult> ValidateAsync(AdditionalResponse response, IValidatorConfig config) { throw new NotImplementedException(); }
}