using PrettyBots.Interactions.Abstraction.Model.Responses;
using PrettyBots.Validators.Abstraction.Exceptions;

namespace PrettyBots.Attributes.Validators;

/// <summary>
/// Specifies that the validator can accept all the config type, as long as the type of config
/// has a type parameter set to the response the validator validates.
/// provided type of config.
/// If the type of <see cref="IResponseModel.Config"/> hasn't been declared
/// as the valid config for the <see cref="IResponseModel{TResponse}.ResponseValidatorType"/>,
/// the <see cref="ConfigurationNotSupportedException{TResponse}"/> will be thrown.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ConfigurableWithAnyOfMyTypeAttribute : Attribute
{
}