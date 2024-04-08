using PrettyBots.Environment;

namespace PrettyBots.Interactions;

public interface IInteractionService<in TMessage>
    where TMessage : IUserMessage
{
    public Task HandleUserMessage(TMessage message, CancellationToken token = default);
}
