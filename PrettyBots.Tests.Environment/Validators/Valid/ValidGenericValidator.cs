using PrettyBots.Attributes.Validators;
using PrettyBots.Tests.Environment.Responses;
using PrettyBots.Validators.Abstraction;
using PrettyBots.Validators.Abstraction.Model;

namespace PrettyBots.Tests.Environment.Validators.Valid;

/// <summary>
/// Test valid text response validator that accept many configs.
/// </summary>
[ConfigurableWithAnyOfMyType]
public class ValidGenericValidator : ResponseValidator<TestResponse>
{
    protected override ValueTask<ValidationResult> ValidateAsync(TestResponse response, IValidatorConfig config) { throw new NotImplementedException(); }
}