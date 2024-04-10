using PrettyBots.Interactions.Validators.Abstraction;
using PrettyBots.Tests.Environment.Responses;

namespace PrettyBots.Tests.Environment.Validators.Invalid;

public class InvalidExceptionConstructorValidator : ResponseValidator<AdditionalResponse>
{
    public InvalidExceptionConstructorValidator()
    {
        throw new IgnorableException();
    }
    
    protected override ValueTask<bool> ValidateAsync(AdditionalResponse response, IValidatorConfig config) { throw new NotImplementedException(); }
}