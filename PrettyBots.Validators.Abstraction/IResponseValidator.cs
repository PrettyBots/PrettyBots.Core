using PrettyBots.Environment;
using PrettyBots.Validators.Abstraction.Exceptions;
using PrettyBots.Validators.Abstraction.Model;

namespace PrettyBots.Validators.Abstraction;

/// <summary>
/// Validates general <see cref="IUserResponse"/> using any <see cref="IValidatorConfig"/>.
/// It is advise to avoid using this interface, unless one wants to create default
/// not type-safe validator that one will be able to use with any response model.
/// </summary>
public interface IResponseValidator
{
    public ValueTask<ValidationResult> ValidateResponseAsync(IUserResponse response, IValidatorConfig config);
}

/// <summary>
/// Validates specific or generic user response against a rules declared in this validator,
/// using the validator config that was specified in the response model.
/// </summary>
/// <remarks>
/// It is advised to use <see cref="ResponseValidator{TResponse}"/>, for it implements
/// methods of this interface and underlying non-generic interface properly. 
/// </remarks>
/// <typeparam name="TResponse">
/// Acts as a warrant that the response in the <see cref="ValidateResponseAsync"/> method
/// will be of that type, and the config will also be for that only type.
/// That warranty should be implemented by interaction processor.
/// </typeparam>
public interface IResponseValidator<in TResponse> : IResponseValidator
    where TResponse : IUserResponse
{
    /// <summary>
    /// Validates the response returning the validation result.
    /// Uses configuration that has been set up in the
    /// config in the response model.
    /// </summary>
    /// <exception cref="ConfigurationNotSupportedException{TResponse}">
    /// Is occurred when this validator cannot handle the provided config type.
    /// </exception>
    public ValueTask<ValidationResult> ValidateResponseAsync(TResponse response, IValidatorConfig config);
}

