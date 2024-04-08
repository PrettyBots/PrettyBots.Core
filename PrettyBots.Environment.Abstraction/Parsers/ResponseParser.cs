using MorseCode.ITask;

namespace PrettyBots.Environment.Parsers;

/// <summary>
/// Parses the specified <see cref="TMessage"/> into the specified <see cref="TResponse"/>.  
/// </summary>
public abstract class ResponseParser<TMessage, TResponse> : IResponseParser<TResponse>
    where TMessage  : IUserMessage
    where TResponse : IUserResponse
{
    public bool CanParse(IUserMessage message)
    {
        return CanParse((TMessage)message);
    }

    public ITask<TResponse> ParseResponseAsync(IUserMessage message, 
        CancellationToken token = default)
    {
        return ParseResponseAsync((TMessage)message, token);
    }

    /// <inheritdoc cref="CanParse"/> 
    protected abstract bool CanParse(TMessage telegramMessage);

    /// <inheritdoc cref="ParseResponseAsync"/> 
    protected abstract ITask<TResponse> ParseResponseAsync(TMessage telegramMessage,
        CancellationToken token = default);
}