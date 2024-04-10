using System.ComponentModel.DataAnnotations;

using PrettyBots.Environment;
using PrettyBots.Environment.Parsers;
using PrettyBots.Interactions.Model.Responses;
using PrettyBots.Interactions.Validators.Abstraction;
using PrettyBots.Model.Responses;
using PrettyBots.Storages.Abstraction.Model;

namespace PrettyBots.Interactions.Builders.InteractionResponses;

/// <summary>
/// Is used to build instances of the <see cref="BasicResponseModel{TResponse}"/>.
/// </summary>
public class ResponseModelBuilder<TMessage, TResponse> : IResponseModelBuilder<TResponse>
    where TMessage  : class, IUserMessage, new()
    where TResponse : class, IUserResponse, new()
{
    private Type? _parserType;
    private Type? _validatorType;
    private readonly string _key;
    private IValidatorConfig<TResponse>? _config;
    private IResponseValidator<TResponse>? _validator;
    
    protected ResponseModelBuilder(string key)
    {
        _key = key;
    }

    /// <summary>
    /// Initiates the building process with the key of the build response.
    /// </summary>
    public static ResponseModelBuilder<TMessage, TResponse> WithKey(string key)
    {
        return new ResponseModelBuilder<TMessage, TResponse>(key);
    }

    /// <inheritdoc cref="WithParser(System.Type)"/>
    public ResponseModelBuilder<TMessage, TResponse> WithParser<TParser>()
        where TParser : ResponseParser<TMessage, TResponse>
    {
        return WithParser(typeof(TParser));
    } 
    
    /// <summary>
    /// Sets the built response model parser type to the specified value.
    /// </summary>
    public ResponseModelBuilder<TMessage, TResponse> WithParser(Type parserType)
    {
        _parserType = parserType;
        return this;
    }
    
    /// <summary>
    /// Sets the <see cref="Validator"/>
    /// to the specified value.
    /// </summary>
    public ResponseModelBuilder<TMessage, TResponse> WithValidator(
        IResponseValidator<TResponse> validator)
    {
        _validator = validator;
        return this;
    }
    
    /// <summary>
    /// Sets the <see cref="IValidatableResponseModel.ResponseValidatorType"/>.
    /// </summary>
    public ResponseModelBuilder<TMessage, TResponse> WithValidator(Type validatorType)
    {
        _validatorType = validatorType;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="IValidatableResponseModel{TResponse}.ResponseValidatorType"/>.
    /// </summary>
    public ResponseModelBuilder<TMessage, TResponse> WithValidator<TValidator>()
        where TValidator : IResponseValidator<TResponse>
    {
        _validatorType = typeof(TValidator);
        return this;
    }

    /// <summary>
    /// Sets the <see cref="IValidatableResponseModel{TResponse}.ResponseValidatorConfig"/>.
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public ResponseModelBuilder<TMessage, TResponse> WithConfig(IValidatorConfig<TResponse> config)
    {
        _config = config;
        return this;
    }


    /// <summary>
    /// Build the response.
    /// </summary>
    public IResponseModel Build()
    {
        if (_validator is null && _validatorType is null) {
            return new BasicResponseModel<TResponse>(_key, _parserType);
        }
        
        return _validatorType is not null 
            ? new ValidatableResponseModel<TResponse>(_key, _parserType, _validatorType, 
                _config) 
            : new ValidatableResponseModel<TResponse>(_key, _parserType, _validator!, _config);
    }
}