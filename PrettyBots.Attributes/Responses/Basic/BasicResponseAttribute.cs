using PrettyBots.Environment.Parsers;
using PrettyBots.Interactions.Abstraction.Model.Responses;
using PrettyBots.Validators.Abstraction;

namespace PrettyBots.Attributes.Responses.Basic;

/// <summary>
/// Adds the basic response as an available response for an interaction.
/// Is equivalent to using <see cref="ResponseModelBuilder{TResponse}"/>,
/// providing the response type and the optional parser.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class BasicResponseAttribute : ResponseBaseAttribute
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
    public BasicResponseAttribute(string key, Type responseType) 
        : base(key, responseType)
    {
    }

    public override IValidatorConfig? CreateConfig() => null;
}