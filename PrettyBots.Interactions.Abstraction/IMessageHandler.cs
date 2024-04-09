using PrettyBots.Environment;

namespace PrettyBots.Interactions.Abstraction;

/// <summary>
/// Part of the interactions service that handles user messages.
/// </summary>
public interface IMessageHandler<in TMessage>
    where TMessage : class, IUserMessage
{
    public Task HandleUserMessage(TMessage message, CancellationToken token = default);
}