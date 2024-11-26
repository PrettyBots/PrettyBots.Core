namespace PrettyBots.Environment.Responses.TextBased;

public interface ITextBasedResponse : IUserResponse
{
    public string Text { get; }
}