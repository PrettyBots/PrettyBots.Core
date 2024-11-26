using PrettyBots.Environment.Responses;
using PrettyBots.Environment.Responses.TextBased;
using PrettyBots.Interactions.Validators.Abstraction;

namespace PrettyBots.Tests.Environment.Validators.Invalid;

/// <summary>
/// Tests abstract validator definition.
/// </summary>
public abstract class InvalidAbstractValidator : ResponseValidator<TextResponse>
{
    protected override ValueTask<bool> ValidateAsync(TextResponse response, IValidatorConfig config) { throw new NotImplementedException(); }
}