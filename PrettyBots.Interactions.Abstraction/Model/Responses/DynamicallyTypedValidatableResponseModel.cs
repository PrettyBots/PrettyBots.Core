using PrettyBots.Environment;
using PrettyBots.Validators.Abstraction;

namespace PrettyBots.Interactions.Abstraction.Model.Responses;

public class DynamicallyTypedValidatableResponseModel : IValidatableResponseModel
{
    public DynamicallyTypedValidatableResponseModel(string key, Type responseType,
        Type? responseParserType, Type? responseValidatorType, 
        IResponseValidator? validator, IValidatorConfig? config)
    {
        Key                   = key;
        Config                = config;
        ResponseType          = responseType;
        ResponseParserType    = responseParserType;
        ResponseValidatorType = responseValidatorType;

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

    public DynamicallyTypedValidatableResponseModel CreateInstanceOfGenericType()
    {
        return (DynamicallyTypedValidatableResponseModel)Activator.CreateInstance(
            typeof(DynamicallyTypedValidatableResponseModel<>).MakeGenericType(ResponseType),
            Key, ResponseType, ResponseParserType, ResponseValidatorType, Validator,
            Config)!;
    }
}

internal class DynamicallyTypedValidatableResponseModel<TResponse> : DynamicallyTypedValidatableResponseModel,
    IValidatableResponseModel<TResponse>
    where TResponse : IUserResponse
{
    public DynamicallyTypedValidatableResponseModel(string key, Type responseType, 
        Type? responseParserType, Type? responseValidatorType, 
        IResponseValidator<TResponse>? validator, IValidatorConfig<TResponse>? config) 
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