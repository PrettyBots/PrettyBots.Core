using MorseCode.ITask;

using PrettyBots.Environment.Parsers.Model;

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
    /// <summary>
    /// Implements rapid way of determining whether this message
    /// is parsable into specified <see cref="TResponse"/> and also
    /// if this parser can parse this message in general.
    /// </summary>
    /// <param name="message">Message that was sent by the user.</param>
    public bool CanParse(IUserMessage message);

    /// <summary>
    /// Parses the message that was sent by the user
    /// into the response of the specified type.
    /// </summary>
    public ITask<ParsingResult> ParseResponseAsync(IUserMessage message, 
        CancellationToken token = default);
}