using PrettyBots.Environment;
using PrettyBots.Interactions.Validators.Abstraction;

namespace PrettyBots.Model.Responses;

/// <summary>
/// Represents a response model that can be validated, thus has the validator option
/// and a config that will be used for validation.  
/// </summary>
public interface IValidatableResponseModel : IResponseModel
{
    /// <summary>
    /// If set, determines the implementation type of the <see cref="Validator"/>
    /// and will be used to instantiate new instances of the validator during the loading.
    /// If not set, the <see cref="Validator"/>, if provided, will be used instead to
    /// validate the response.
    /// Should be a type derived from <see cref="IResponseValidator"/>.
    /// </summary>
    public Type? ResponseValidatorType { get; }
    
    /// <summary>
    /// Determines the validator that will be used to validate parsed response.
    /// Can be null, if the loading process has not yet been completed.
    /// </summary>
    public IResponseValidator Validator { get; set; }
    
    /// <summary>
    /// Gets transferred into <see cref="IResponseValidator.ValidateResponseAsync"/>
    /// in order to configure the validation process.
    /// </summary>
    public IValidatorConfig? Config { get; }
}

/// <summary>
/// Generically restricted <see cref="IValidatableResponseModel"/> that unifies response type
/// in order to use in the validator.
/// </summary>
/// <typeparam name="TResponse">Type of the response this model describes</typeparam>
public interface IValidatableResponseModel<in TResponse> : IValidatableResponseModel
    where TResponse : IUserResponse
{
    public IResponseValidator<TResponse> ResponseValidator { get; }
    
    public IValidatorConfig<TResponse>? ResponseValidatorConfig { get; }
}