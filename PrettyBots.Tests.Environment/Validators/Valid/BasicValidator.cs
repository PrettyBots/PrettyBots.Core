using PrettyBots.Environment;
using PrettyBots.Interactions.Validators.Abstraction;
using PrettyBots.Interactions.Validators.Abstraction.Model;

namespace PrettyBots.Tests.Environment.Validators.Valid;

public class BasicValidator : IResponseValidator
{
    public ValueTask<ValidationResult> ValidateResponseAsync(IUserResponse response, IValidatorConfig config) { throw new NotImplementedException(); }
}