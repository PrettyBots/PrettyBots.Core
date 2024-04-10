using PrettyBots.Attributes.Validators;
using PrettyBots.Interactions.Validators.Abstraction;
using PrettyBots.Tests.Environment.Responses;

namespace PrettyBots.Tests.Environment.Validators.Invalid;

[ConfigurableWith(typeof(IValidatorConfig<>))]
[ConfigurableWith(typeof(IResponseValidator<>))]
public class InvalidConfigValidator : ResponseValidator<IAbstractResponse>
{
    protected override ValueTask<bool> ValidateAsync(IAbstractResponse response, IValidatorConfig config) { throw new NotImplementedException(); }
}