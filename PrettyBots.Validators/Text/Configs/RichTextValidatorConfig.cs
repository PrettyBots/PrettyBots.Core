using PrettyBots.Attributes.Responses;
using PrettyBots.Environment.Responses.TextBased;
using PrettyBots.Validators.Abstraction;
using PrettyBots.Validators.Text.Configs.Enums;

namespace PrettyBots.Validators.Text.Configs;

/// <summary>
/// Configures text interaction response.
/// </summary>
public class RichTextValidatorConfig : IValidatorConfig<ITextBasedResponse>
{
    // TODO: Change to default telegram max value
    public const uint TEXT_RESPONSE_MAX_VALUE = UInt32.MaxValue;
    
    public uint MinValue { get; }

    public uint MaxValue { get; }
    
    public string Prefix { get; }

    public FormatType FormatType { get; }

    public RichTextValidatorConfig(uint minValue = 1, uint maxValue = TEXT_RESPONSE_MAX_VALUE, string prefix = "",
        FormatType formatType = FormatType.None)
    {
        MinValue   = minValue;
        MaxValue   = maxValue;
        Prefix     = prefix;
        FormatType = formatType;
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class RichTextResponseAttribute : ValidatableResponseAttribute
{
    public uint MinValue { get; set; } = 0;
    public uint MaxValue { get; set; } = RichTextValidatorConfig.TEXT_RESPONSE_MAX_VALUE;
    public string Prefix { get; set; } = "";
    public FormatType FormatType { get; set; } = FormatType.None;
    
    public RichTextResponseAttribute(string key, Type? responseType = null) 
        : base(key, responseType ?? typeof(TextResponse), typeof(RichTextResponseValidator))
    {
    }

    public override IValidatorConfig? CreateConfig()
    {
        return new RichTextValidatorConfig(MinValue, MaxValue, Prefix, FormatType);
    }
} 