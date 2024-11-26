using PrettyBots.Environment;
using PrettyBots.Interactions.Abstraction.EventHandlers;

namespace PrettyBots.Interactions.Abstraction;

/// <summary>
/// Part of the interactions service that handles user messages.
/// </summary>
public interface IMessageHandler<in TMessage>
    where TMessage : class, IUserMessage
{
    public Task HandleUserMessageAsync(TMessage message, CancellationToken token = default);

    public event IncorrectUserMessageEventHandler OnIncorrectUserMessage;
}