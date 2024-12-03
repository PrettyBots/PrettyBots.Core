using PrettyBots.Attributes.Validators;
using PrettyBots.Environment.Responses;
using PrettyBots.Environment.Responses.TextBased;
using PrettyBots.Tests.Environment.Validators.Configs;
using PrettyBots.Validators.Abstraction;
using PrettyBots.Validators.Abstraction.Model;

namespace PrettyBots.Tests.Environment.Validators.Invalid;


[ConfigurableWith(typeof(ImageTestConfig))]
public class InvalidConfigValidator2 : ResponseValidator<TextResponse>
{
    protected override ValueTask<ValidationResult> ValidateAsync(TextResponse response, IValidatorConfig config) { throw new NotImplementedException(); }
}