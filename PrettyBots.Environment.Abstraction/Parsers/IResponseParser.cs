using MorseCode.ITask;

namespace PrettyBots.Environment.Parsers;

/// <summary>
/// Parses the general <see cref="IUserMessage"/> into specified <see cref="TResponse"/>.
/// </summary>
/// <remarks>
/// Parsers that are direct implementations of this interface will not be loaded
/// into the registry on any loading process.
/// Consider using <see cref="ResponseParser{TMessage,TResponse}"/> instead.
/// </remarks>
public interface IResponseParser<out TResponse>
    where TResponse : IUserResponse
{
    public bool CanParse(IUserMessage message);

    public ITask<TResponse> ParseResponseAsync(IUserMessage message, 
        CancellationToken token = default);
}