using PrettyBots.Attributes.Common;
using PrettyBots.Environment;
using PrettyBots.Interactions.Validators.Abstraction;
using PrettyBots.Interactions.Validators.Abstraction.Model;

namespace PrettyBots.Tests.Environment.Validators.Invalid;

[Ignore(Reason = "")]
public class NeverLoadingValidator : ResponseValidator<IUserResponse>
{
    protected override ValueTask<ValidationResult> ValidateAsync(IUserResponse response, IValidatorConfig config) { throw new NotImplementedException(); }
}