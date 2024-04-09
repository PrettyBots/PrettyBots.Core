using PrettyBots.Attributes.Validators;
using PrettyBots.Environment.Responses;
using PrettyBots.Interactions.Validators.Abstraction;
using PrettyBots.Interactions.Validators.Configs;

namespace PrettyBots.Interactions.Validators;

[ConfigurableWith(typeof(RichTextValidatorConfig))]
public class RichTextResponseValidator : ResponseValidator<TextResponse>
{
    protected override ValueTask<bool> ValidateAsync(TextResponse response, IValidatorConfig config)
    {
        return ValueTask.FromResult(true);
    }
}