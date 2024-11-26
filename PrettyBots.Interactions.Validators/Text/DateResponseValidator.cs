using PrettyBots.Attributes.Validators;
using PrettyBots.Environment.Responses;
using PrettyBots.Interactions.Validators.Abstraction;
using PrettyBots.Interactions.Validators.Abstraction.Model;
using PrettyBots.Interactions.Validators.Text.Configs;
using PrettyBots.Interactions.Validators.Text.Configs.Enums;

namespace PrettyBots.Interactions.Validators.Text;

[ConfigurableWith(typeof(DateValidatorConfig))]
public class DateResponseValidator : ResponseValidator<DateResponse>
{
    protected async override ValueTask<ValidationResult> ValidateAsync(DateResponse response, IValidatorConfig config)
    {
        DateValidatorConfig typedConfig = (DateValidatorConfig)config;
        
        return typedConfig.DateComparison switch {
            DateComparison.MoreThenNow => response.Date > DateTime.Now
                ? ValidationResult.Ok()
                : ValidationResult.Error(ValidatorErrorType.IncorrectUserTextMessage,
                    "Received datetime doesn't math provided option"),
            DateComparison.LessThenNow => response.Date < DateTime.Now
                ? ValidationResult.Ok()
                : ValidationResult.Error(ValidatorErrorType.IncorrectUserTextMessage,
                    "Received datetime doesn't math provided option"),
            DateComparison.Equal => response.Date == DateTime.Now
                ? ValidationResult.Ok()
                : ValidationResult.Error(ValidatorErrorType.IncorrectUserTextMessage,
                    "Received datetime doesn't math provided option"),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}