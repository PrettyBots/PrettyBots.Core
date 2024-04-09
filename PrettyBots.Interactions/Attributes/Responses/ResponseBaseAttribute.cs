using PrettyBots.Interactions.Validators.Abstraction;
using PrettyBots.Model.Responses;

namespace PrettyBots.Interactions.Attributes.Responses;

public abstract class ResponseBaseAttribute : Attribute
{
    public string Key { get; }
    public Type ResponseType { get; }
    public Type? ParserType { get; }
    public Type? ValidatorType { get; }

    /// <summary>
    /// The config factory that is used to instantiate config instances
    /// based on provided arguments in a derived attributes.
    /// </summary>
    public abstract IValidatorConfig? CreateConfig();
    
    protected ResponseBaseAttribute(string key, Type responseType, Type? parserType,
        Type? validatorType)
    {
        Key           = key;
        ParserType    = parserType;
        ResponseType  = responseType;
        ValidatorType = validatorType;
    }

    public abstract IResponseModel CreateModel();
}