using MorseCode.ITask;

using PrettyBots.Environment.Parsers.Model;

namespace PrettyBots.Environment.Parsers;

/// <summary>
/// Parses the specified <see cref="TMessage"/> into the specified <see cref="TResponse"/>.  
/// </summary>
/// <remarks>
/// In order to be detected and/or loaded without errors, should have the
/// <typeparamref name="TMessage"/> type set to the <see cref="IEnvironment.MessageType"/>
/// of the environment it operates in.
/// </remarks>
public abstract class ResponseParser<TMessage, TResponse> : IResponseParser<TResponse>
    where TMessage  : class, IUserMessage
    where TResponse : class, IUserResponse
{
    public bool CanParse(IUserMessage message)
    {
        return CanParse((TMessage)message);
    }

    public ITask<ParsingResult> ParseResponseAsync(IUserMessage message, 
        CancellationToken                                                  token = default)
    {
        return ParseResponseAsync((TMessage)message, token);
    }

    /// <inheritdoc cref="CanParse"/> 
    protected abstract bool CanParse(TMessage message);

    /// <inheritdoc cref="ParseResponseAsync"/> 
    protected abstract ITask<ParsingResult> ParseResponseAsync(TMessage message,
        CancellationToken token = default);
}