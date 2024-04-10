using PrettyBots.Environment;
using PrettyBots.Interactions.Validators.Abstraction;
using PrettyBots.Tests.Environment.Responses;

namespace PrettyBots.Tests.Environment.Validators.Valid;

public class BasicTypedValidator : IResponseValidator<TestResponse>
{
    public ValueTask<bool> ValidateResponseAsync(TestResponse response, IValidatorConfig config) { throw new NotImplementedException(); }

    public ValueTask<bool> ValidateResponseAsync(IUserResponse response, IValidatorConfig config) { throw new NotImplementedException(); }
}