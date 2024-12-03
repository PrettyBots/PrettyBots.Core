using PrettyBots.Attributes.Validators;
using PrettyBots.Environment.Responses;
using PrettyBots.Validators.Abstraction;
using PrettyBots.Validators.Abstraction.Model;
using PrettyBots.Validators.Text.Configs;
using PrettyBots.Validators.Text.Configs.Enums;

namespace PrettyBots.Validators.Text;

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