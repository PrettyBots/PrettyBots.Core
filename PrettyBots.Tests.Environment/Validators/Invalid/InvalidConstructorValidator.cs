using JetBrains.Annotations;

using PrettyBots.Tests.Environment.Responses;
using PrettyBots.Validators.Abstraction;
using PrettyBots.Validators.Abstraction.Model;

namespace PrettyBots.Tests.Environment.Validators.Invalid;

public class InvalidConstructorValidator : ResponseValidator<AdditionalResponse>
{
    public InvalidConstructorValidator([UsedImplicitly]string e)
    {
        
    }
    
    protected override ValueTask<ValidationResult> ValidateAsync(AdditionalResponse response, IValidatorConfig config) { throw new NotImplementedException(); }
}