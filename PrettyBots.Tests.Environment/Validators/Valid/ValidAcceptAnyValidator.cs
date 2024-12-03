using PrettyBots.Attributes.Validators;
using PrettyBots.Environment.Responses;
using PrettyBots.Environment.Responses.TextBased;
using PrettyBots.Validators.Abstraction;
using PrettyBots.Validators.Abstraction.Model;

namespace PrettyBots.Tests.Environment.Validators.Valid;

/// <summary>
/// Tests valid validator that accept any config.
/// </summary>
[ConfigurableWithAny]
public class ValidAcceptAnyValidator : ResponseValidator<TextResponse>
{
    protected override ValueTask<ValidationResult> ValidateAsync(TextResponse response, IValidatorConfig config) { throw new NotImplementedException(); }
}