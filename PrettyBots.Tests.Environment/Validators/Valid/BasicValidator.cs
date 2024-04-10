using PrettyBots.Environment;
using PrettyBots.Interactions.Validators.Abstraction;

namespace PrettyBots.Tests.Environment.Validators.Valid;

public class BasicValidator : IResponseValidator
{
    public ValueTask<bool> ValidateResponseAsync(IUserResponse response, IValidatorConfig config) { throw new NotImplementedException(); }
}