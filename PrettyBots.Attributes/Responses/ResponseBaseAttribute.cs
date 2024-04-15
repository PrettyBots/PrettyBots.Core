using PrettyBots.Interactions.Abstraction.Model.Responses;
using PrettyBots.Interactions.Validators.Abstraction;

namespace PrettyBots.Attributes.Responses;

public abstract class ResponseBaseAttribute : Attribute
{
    public string Key { get; }
    public Type ResponseType { get; }
    public Type? ParserType { get; protected set; }
    public Type? ValidatorType { get; protected set; }

    /// <summary>
    /// The config factory that is used to instantiate config instances
    /// based on provided arguments in a derived attributes.
    /// </summary>
    public abstract IValidatorConfig? CreateConfig();
    
    protected ResponseBaseAttribute(string key, Type responseType)
    {
        Key          = key;
        ResponseType = responseType;
    }

    public virtual IResponseModel CreateModel()
    {
        return new DynamicallyTypedResponseModel(Key, ResponseType, 
                ParserType, ValidatorType, null, CreateConfig())
            .CreateInstanceOfGenericType();
    }
}