namespace PrettyBots.Interactions.Abstraction.Model.Responses;

public class DynamicallyTypedResponseModel : IResponseModel
{
    public string Key                { get; }
    public Type   ResponseType       { get; }
    public Type?  ResponseParserType { get; set; }

    public DynamicallyTypedResponseModel(string key, Type responseType, Type? responseParserType)
    {
        Key                = key;
        ResponseType       = responseType;
        ResponseParserType = responseParserType;
    }
}
