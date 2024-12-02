using PrettyBots.Attributes.Validators;
using PrettyBots.Interactions.Validators.Abstraction;
using PrettyBots.Interactions.Validators.Abstraction.Model;
using PrettyBots.Tests.Environment.Responses;

namespace PrettyBots.Tests.Environment.Validators.Valid;

/// <summary>
/// Test valid text response validator that accept many configs.
/// </summary>
[ConfigurableWithAnyOfMyType]
public class ValidGenericValidator : ResponseValidator<TestResponse>
{
    protected override ValueTask<ValidationResult> ValidateAsync(TestResponse response, IValidatorConfig config) { throw new NotImplementedException(); }
}