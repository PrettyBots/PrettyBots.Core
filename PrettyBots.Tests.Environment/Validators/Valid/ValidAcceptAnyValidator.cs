using PrettyBots.Attributes.Validators;
using PrettyBots.Environment.Responses;
using PrettyBots.Interactions.Validators.Abstraction;

namespace PrettyBots.Tests.Environment.Validators.Valid;

/// <summary>
/// Tests valid validator that accept any config.
/// </summary>
[ConfigurableWithAny]
public class ValidAcceptAnyValidator : ResponseValidator<TextResponse>
{
    protected override ValueTask<bool> ValidateAsync(TextResponse response, IValidatorConfig config) { throw new NotImplementedException(); }
}