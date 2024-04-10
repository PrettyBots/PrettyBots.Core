using PrettyBots.Attributes.Validators;
using PrettyBots.Environment.Responses;
using PrettyBots.Interactions.Validators.Abstraction;
using PrettyBots.Tests.Environment.Validators.Configs;

namespace PrettyBots.Tests.Environment.Validators.Invalid;


[ConfigurableWith(typeof(ImageTestConfig))]
public class InvalidConfigValidator2 : ResponseValidator<TextResponse>
{
    protected override ValueTask<bool> ValidateAsync(TextResponse response, IValidatorConfig config) { throw new NotImplementedException(); }
}