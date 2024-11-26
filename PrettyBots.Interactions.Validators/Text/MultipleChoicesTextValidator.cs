using PrettyBots.Attributes.Validators;
using PrettyBots.Environment.Responses;
using PrettyBots.Environment.Responses.TextBased;
using PrettyBots.Interactions.Validators.Abstraction;
using PrettyBots.Interactions.Validators.Abstraction.Model;
using PrettyBots.Interactions.Validators.Text.Configs;

namespace PrettyBots.Interactions.Validators.Text;

[ConfigurableWith(typeof(MultipleChoicesTextValidatorConfig))]
public class MultipleChoicesTextValidator : ResponseValidator<ITextBasedResponse>
{
    protected async override ValueTask<ValidationResult> ValidateAsync(ITextBasedResponse response, IValidatorConfig config)
    {
        MultipleChoicesTextValidatorConfig typedConfig = (MultipleChoicesTextValidatorConfig)config;
        
        return typedConfig.Choices.Contains(response.Text) ? ValidationResult.Ok() 
            : ValidationResult.Error(
                ValidatorErrorType.IncorrectUserTextMessage, 
                "Received text message doesn't matched any provided options");
    }
}