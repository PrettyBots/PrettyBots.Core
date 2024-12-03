using PrettyBots.Attributes.Validators;
using PrettyBots.Tests.Environment.Responses;
using PrettyBots.Validators.Abstraction;
using PrettyBots.Validators.Abstraction.Model;

namespace PrettyBots.Tests.Environment.Validators.Invalid;

[ConfigurableWith(typeof(IValidatorConfig<>))]
[ConfigurableWith(typeof(IResponseValidator<>))]
public class InvalidConfigValidator : ResponseValidator<IAbstractResponse>
{
    protected override ValueTask<ValidationResult> ValidateAsync(IAbstractResponse response, IValidatorConfig config) { throw new NotImplementedException(); }
}