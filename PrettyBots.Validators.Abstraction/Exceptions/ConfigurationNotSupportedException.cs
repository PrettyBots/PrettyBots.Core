using PrettyBots.Environment;

namespace PrettyBots.Validators.Abstraction.Exceptions;

/// <summary>
/// The exception that is thrown by <see cref="IResponseValidator{TResponse}.ValidateResponseAsync"/>
/// in case the configuration set for the <see cref="IResponseModel{TResponse}"/> cannot be handled
/// properly using current validator.
/// </summary>
public class ConfigurationNotSupportedException<TResponse> : Exception
    where TResponse : IUserResponse
{
    public IResponseValidator<TResponse> Validator;

    public IValidatorConfig<TResponse> Config;

    public ConfigurationNotSupportedException(IResponseValidator<TResponse> validator, 
        IValidatorConfig<TResponse> config)
        : base($"Validator {validator.GetType()} is not able to handle configs of the " +
               $"{config.GetType()} type")
    {
        Config    = config;
        Validator = validator;
    }
}