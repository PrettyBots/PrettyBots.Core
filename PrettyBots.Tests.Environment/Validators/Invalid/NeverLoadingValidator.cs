using PrettyBots.Attributes.Common;
using PrettyBots.Environment;
using PrettyBots.Validators.Abstraction;
using PrettyBots.Validators.Abstraction.Model;

namespace PrettyBots.Tests.Environment.Validators.Invalid;

[Ignore(Reason = "")]
public class NeverLoadingValidator : ResponseValidator<IUserResponse>
{
    protected override ValueTask<ValidationResult> ValidateAsync(IUserResponse response, IValidatorConfig config) { throw new NotImplementedException(); }
}