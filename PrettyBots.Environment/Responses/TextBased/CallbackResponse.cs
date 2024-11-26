namespace PrettyBots.Environment.Responses.TextBased;

public class CallbackResponse : ITextBasedResponse
{
    public string CallbackData { get; set; }
    public string Text => CallbackData;
    public IEnvironment Environment { get; set; } = null!;

    public CallbackResponse(string callbackData)
    {
        CallbackData = callbackData;
    }

}
