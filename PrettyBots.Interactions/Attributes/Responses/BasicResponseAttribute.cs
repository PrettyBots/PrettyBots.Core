using PrettyBots.Environment.Parsers;
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
public sealed class BasicResponseAttribute : ResponseBaseAttribute
{
    /// <summary>
    /// Registers a response.
    /// </summary>
    /// <param name="key"><see cref="IResponseModel.Key"/> value</param>
    /// <param name="responseType">
    /// Is equivalent to TResponse type in <see cref="IResponseModel{TResponse}"/>
    /// </param>
    /// <param name="parserType">
    /// Should be a type that implements <see cref="IResponseParser{TResponse}"/>
    /// with TResponse set to <paramref name="responseType"/>.
    /// </param>
    public BasicResponseAttribute(string key, Type responseType, Type? parserType = null) 
        : base(key, responseType, parserType, null)
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