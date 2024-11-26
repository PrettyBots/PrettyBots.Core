namespace PrettyBots.Environment.Telegram;

public class TelegramEnvironment : IEnvironment
{
    public Type MessageType => typeof(TelegramUserMessage);
}