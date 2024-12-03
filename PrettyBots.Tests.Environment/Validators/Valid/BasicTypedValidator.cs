using PrettyBots.Environment;
using PrettyBots.Tests.Environment.Responses;
using PrettyBots.Validators.Abstraction;
using PrettyBots.Validators.Abstraction.Model;

namespace PrettyBots.Tests.Environment.Validators.Valid;

public class BasicTypedValidator : IResponseValidator<TestResponse>
{
    public ValueTask<ValidationResult> ValidateResponseAsync(TestResponse response, IValidatorConfig config) { throw new NotImplementedException(); }

    public ValueTask<ValidationResult> ValidateResponseAsync(IUserResponse response, IValidatorConfig config) { throw new NotImplementedException(); }
}