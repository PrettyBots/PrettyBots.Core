using PrettyBots.Interactions.Builders.InteractionResponses;
using PrettyBots.Interactions.Model.Responses;
using PrettyBots.Interactions.Validators.Abstraction;
using PrettyBots.Model.Responses;

namespace PrettyBots.Interactions.Attributes.Responses;

/// <summary>
/// Adds the basic response as an available response for an interaction.
/// Is equivalent to using <see cref="ResponseModelBuilder{TResponse}"/>,
/// providing the response type and the optional parser.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class ValidatableResponseAttribute : ResponseBaseAttribute
{
    public ValidatableResponseAttribute(string key, Type responseType, Type validatorType,
        Type? parserType = null) : base(key, responseType, parserType, validatorType)
    {
    }

    public override IValidatorConfig? CreateConfig() => null;

    public override IResponseModel CreateModel()
    {
        return new DynamicallyTypedResponseModel(Key, ResponseType, ParserType, 
                ValidatorType, null, CreateConfig())
            .CreateInstanceOfGenericType();
    }
}