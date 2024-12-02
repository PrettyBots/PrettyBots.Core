using JetBrains.Annotations;

using PrettyBots.Interactions.Validators.Abstraction;
using PrettyBots.Interactions.Validators.Abstraction.Model;
using PrettyBots.Tests.Environment.Responses;

namespace PrettyBots.Tests.Environment.Validators.Invalid;

public class InvalidConstructorValidator : ResponseValidator<AdditionalResponse>
{
    public InvalidConstructorValidator([UsedImplicitly]string e)
    {
        
    }
    
    protected override ValueTask<ValidationResult> ValidateAsync(AdditionalResponse response, IValidatorConfig config) { throw new NotImplementedException(); }
}