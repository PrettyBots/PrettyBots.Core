using MorseCode.ITask;

using PrettyBots.Environment.Parsers;
using PrettyBots.Environment.Parsers.Model;
using PrettyBots.Environment.Responses;

using Telegram.Bot.Types.Enums;

namespace PrettyBots.Environment.Telegram.Parsers;

public class MessageResponseParser : ResponseParser<TelegramUserMessage, MessageResponse>
{
    protected override bool CanParse(TelegramUserMessage message) => message.UpdateData.Type == UpdateType.Message;

    protected async override ITask<ParsingResult> ParseResponseAsync(TelegramUserMessage message, CancellationToken token = default)
    {
        return ParsingResult.Ok(new MessageResponse());
    }
}