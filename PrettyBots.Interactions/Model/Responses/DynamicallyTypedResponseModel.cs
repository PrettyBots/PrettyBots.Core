using PrettyBots.Environment;
using PrettyBots.Interactions.Validators.Abstraction;
using PrettyBots.Model.Responses;

namespace PrettyBots.Interactions.Model.Responses;

public class DynamicallyTypedResponseModel : IValidatableResponseModel
{
    public DynamicallyTypedResponseModel(string key, Type responseType, 
        Type? responseParserType, Type? responseValidatorType, 
        IResponseValidator? validator, IValidatorConfig? config)
    {
        Key                   = key;
        ResponseType          = responseType;
        ResponseParserType    = responseParserType;
        ResponseValidatorType = responseValidatorType;
        Config                = config;

        if (validator is not null) {
            Validator = validator;
        }
    }

    public string Key { get; }
    public Type ResponseType { get; }
    public Type? ResponseParserType { get; set; }
    public Type? ResponseValidatorType { get; }
    public IResponseValidator Validator { get; set; } = null!;
    public IValidatorConfig? Config { get; }

    public DynamicallyTypedResponseModel CreateInstanceOfGenericType()
    {
        return (DynamicallyTypedResponseModel)Activator.CreateInstance(
            typeof(DynamicallyTypedResponseModel<>).MakeGenericType(ResponseType),
            Key, ResponseType, ResponseParserType, ResponseValidatorType, Validator,
            Config)!;
    }
}

internal class DynamicallyTypedResponseModel<TResponse> : DynamicallyTypedResponseModel,
    IValidatableResponseModel<TResponse>
    where TResponse : IUserResponse
{
    public DynamicallyTypedResponseModel(string key, Type responseType, Type? responseParserType, 
        Type? responseValidatorType, IResponseValidator<TResponse>? validator, 
        IValidatorConfig<TResponse>? config) 
        : base(key, responseType, responseParserType, responseValidatorType, validator, config)
    {
        ResponseValidatorConfig = config;

        if (validator is not null) {
            ResponseValidator = validator;
        }
    }

    IResponseValidator IValidatableResponseModel.Validator
    {
        get => ResponseValidator;
        set => ResponseValidator = (IResponseValidator<TResponse>)value;
    }

    IValidatorConfig? IValidatableResponseModel.Config => ResponseValidatorConfig;

    public IResponseValidator<TResponse> ResponseValidator { get; private set; } = null!;
    public IValidatorConfig<TResponse>? ResponseValidatorConfig { get; }
}