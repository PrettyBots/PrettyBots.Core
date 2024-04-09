using PrettyBots.Environment;
using PrettyBots.Interactions.Validators.Abstraction;
using PrettyBots.Model.Responses;

namespace PrettyBots.Interactions.Model.Responses;

public class ValidatableResponseModel<TResponse> : BasicResponseModel<TResponse>,
                                                   IValidatableResponseModel<TResponse>
    where TResponse : IUserResponse
{
    
    /// <inheritdoc />
    public Type? ResponseValidatorType { get; }

    IResponseValidator IValidatableResponseModel.Validator
    {
        get => ResponseValidator;
        set => ResponseValidator = (IResponseValidator<TResponse>)value;
    }

    IValidatorConfig? IValidatableResponseModel.Config => ResponseValidatorConfig;

    /// <inheritdoc />
    public IResponseValidator<TResponse> ResponseValidator { get; private set; } = null!;
    
    /// <inheritdoc />
    public IValidatorConfig<TResponse>? ResponseValidatorConfig { get; }

    public ValidatableResponseModel(string key, Type? responseParserType, 
        Type validatorType, IValidatorConfig<TResponse>? config) 
        : base(key, responseParserType)
    {
        ResponseValidatorType   = validatorType;
        ResponseValidatorConfig = config;
    }

    public ValidatableResponseModel(string key, Type? responseParserType,
        IResponseValidator<TResponse> validator, IValidatorConfig<TResponse>? config) 
        : base(key, responseParserType)
    {
        ResponseValidator       = validator;
        ResponseValidatorConfig = config;
    }
}