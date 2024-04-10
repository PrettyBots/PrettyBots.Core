using PrettyBots.Attributes.Common;
using PrettyBots.Environment;
using PrettyBots.Interactions.Validators.Abstraction;

namespace PrettyBots.Tests.Environment.Validators.Invalid;

[Ignore(Reason = "")]
public class NeverLoadingValidator : ResponseValidator<IUserResponse>
{
    protected override ValueTask<bool> ValidateAsync(IUserResponse response, IValidatorConfig config) { throw new NotImplementedException(); }
}