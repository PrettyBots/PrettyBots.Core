using PrettyBots.Environment.Responses;
using PrettyBots.Interactions.Attributes.Responses;
using PrettyBots.Interactions.Validators.Abstraction;

namespace PrettyBots.Interactions.Validators.Configs;

/// <summary>
/// Configures text interaction response.
/// </summary>
public class RichTextValidatorConfig : IValidatorConfig<TextResponse>
{
    // TODO: Change to default telegram max value
    public const uint TEXT_RESPONSE_MAX_VALUE = UInt32.MaxValue;
    
    public uint MinValue { get; }

    public uint MaxValue { get; }

    public RichTextValidatorConfig(uint minValue = 1, uint maxValue = TEXT_RESPONSE_MAX_VALUE)
    {
        MinValue = minValue;
        MaxValue = maxValue;
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class RichTextResponseAttribute : ValidatableResponseAttribute
{
    public uint MinValue { get; set; } = 0;
    public uint MaxValue { get; set; } = RichTextValidatorConfig.TEXT_RESPONSE_MAX_VALUE;
    
    public RichTextResponseAttribute(string key, Type validatorType, Type? parserType = null) 
        : base(key, typeof(TextResponse), validatorType, parserType)
    {
    }

    public override IValidatorConfig? CreateConfig()
    {
        return new RichTextValidatorConfig(MinValue, MaxValue);
    }
} 