using PrettyBots.Validators.Abstraction;

namespace PrettyBots.Attributes.Responses;

/// <summary>
/// Adds the basic response as an available response for an interaction.
/// Is equivalent to using <see cref="ResponseModelBuilder{TResponse}"/>,
/// providing the response type and the optional parser.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class ValidatableResponseAttribute : ResponseBaseAttribute
{
    public ValidatableResponseAttribute(string key, Type responseType, 
        Type validatorType) : base(key, responseType)
    {
        ValidatorType = validatorType;
    }

    public override IValidatorConfig? CreateConfig() => null;
}