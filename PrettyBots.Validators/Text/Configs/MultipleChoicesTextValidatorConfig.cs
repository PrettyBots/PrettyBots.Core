using PrettyBots.Attributes.Responses;
using PrettyBots.Environment.Responses.TextBased;
using PrettyBots.Validators.Abstraction;

namespace PrettyBots.Validators.Text.Configs;

public class MultipleChoicesTextValidatorConfig : IValidatorConfig<ITextBasedResponse>
{
    public string[] Choices { get; }

    public MultipleChoicesTextValidatorConfig(params string[] choices)
    {
        Choices = choices;
    }
    
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class MultipleChoicesResponseAttribute : ValidatableResponseAttribute
{
    public string[] Choices { get; set; }

    public MultipleChoicesResponseAttribute(string key, params string[] choices)
        : base(key, typeof(TextResponse), typeof(MultipleChoicesTextValidator))
    {
        Choices = choices;
    }
    
    public MultipleChoicesResponseAttribute(string key, Type responseType, params string[] choices)
        : base(key, responseType, typeof(MultipleChoicesTextValidator))
    {
        Choices = choices;
    }
    
    public override IValidatorConfig? CreateConfig()
    {
        return new MultipleChoicesTextValidatorConfig(Choices);
    }
}
