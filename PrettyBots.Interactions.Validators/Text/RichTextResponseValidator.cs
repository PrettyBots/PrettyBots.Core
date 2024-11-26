using System.Text.RegularExpressions;

using PrettyBots.Attributes.Validators;
using PrettyBots.Environment.Responses;
using PrettyBots.Environment.Responses.TextBased;
using PrettyBots.Interactions.Validators.Abstraction;
using PrettyBots.Interactions.Validators.Abstraction.Model;
using PrettyBots.Interactions.Validators.Text.Configs;
using PrettyBots.Interactions.Validators.Text.Configs.Enums;

namespace PrettyBots.Interactions.Validators.Text;

[ConfigurableWith(typeof(RichTextValidatorConfig))]
public class RichTextResponseValidator : ResponseValidator<ITextBasedResponse>
{
    protected async override ValueTask<ValidationResult> ValidateAsync(ITextBasedResponse response, IValidatorConfig config)
    {
        RichTextValidatorConfig typedConfig = (RichTextValidatorConfig)config;
        if (response.Text.Length < typedConfig.MinValue) {
            return ValidationResult.Error(ValidatorErrorType.IncorrectUserTextMessage, ""); //TODO:
        }

        if (response.Text.Length > typedConfig.MaxValue) {
            return ValidationResult.Error(ValidatorErrorType.IncorrectUserTextMessage, ""); //TODO:
        }

        if (!response.Text.StartsWith(typedConfig.Prefix)) {
            return ValidationResult.Error(ValidatorErrorType.IncorrectUserTextMessage, ""); //TODO:
        }

        switch (typedConfig.FormatType) {
            case FormatType.None:
                break;
            
            case FormatType.Text:
                if (!Regex.IsMatch(response.Text, @"^[А-Яа-яЁё]+$") || 
                    !Regex.IsMatch(response.Text, @"^[A-Za-z]+$")) {
                    return ValidationResult.Error(ValidatorErrorType.IncorrectUserTextMessage, ""); //TODO:
                }
                break;

            case FormatType.Number:
                if (!Regex.IsMatch(response.Text, @"^\d+$")) {
                    return ValidationResult.Error(ValidatorErrorType.IncorrectUserTextMessage, ""); //TODO:
                }
                break;

            // case FormatType.Date:
            //     string pattern =
            //         @"^(0[1-9]|[12][0-9]|3[01])\.(0[1-9]|1[0-2])\.\d{2} (0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$";
            //     if (!Regex.IsMatch(response.Text, pattern)) {
            //         return ValidationResult.Error(ValidatorErrorType.IncorrectUserTextMessage, ""); //TODO:
            //     }
            //     break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        return ValidationResult.Ok();
    }
}