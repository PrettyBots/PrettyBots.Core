using PrettyBots.Attributes.Responses;
using PrettyBots.Environment.Responses;
using PrettyBots.Validators.Abstraction;
using PrettyBots.Validators.Text.Configs.Enums;

namespace PrettyBots.Validators.Text.Configs;

public class DateValidatorConfig : IValidatorConfig<DateResponse>
{
    public DateComparison DateComparison { get; }

    public DateValidatorConfig(DateComparison dateComparison = DateComparison.MoreThenNow)
    {
        DateComparison = dateComparison;
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class DateResponseAttribute : ValidatableResponseAttribute
{
    public DateComparison DateComparison { get; } = DateComparison.MoreThenNow;
    
    public DateResponseAttribute(string key, Type? responseType = null)
        : base(key, responseType ?? typeof(DateResponse), typeof(DateResponseValidator))
    {
    }
    
    public override IValidatorConfig? CreateConfig()
    {
        return new DateValidatorConfig(DateComparison);
    }
}

