using PrettyBots.Attributes.Validators;
using PrettyBots.Interactions.Validators.Abstraction;
using PrettyBots.Interactions.Validators.Abstraction.Model;
using PrettyBots.Tests.Environment.Responses;

namespace PrettyBots.Tests.Environment.Validators.Invalid;

[ConfigurableWith(typeof(IValidatorConfig<>))]
[ConfigurableWith(typeof(IResponseValidator<>))]
public class InvalidConfigValidator : ResponseValidator<IAbstractResponse>
{
    protected override ValueTask<ValidationResult> ValidateAsync(IAbstractResponse response, IValidatorConfig config) { throw new NotImplementedException(); }
}