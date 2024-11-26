using MorseCode.ITask;

using PrettyBots.Environment.Parsers;
using PrettyBots.Environment.Parsers.Model;
using PrettyBots.Environment.Responses.TextBased;
using PrettyBots.Environment.Telegram;
using PrettyBots.Interactions.Validators.Abstraction;

using Telegram.Bot.Types.Enums;

namespace PrettyBots.Interactions.Telegram.Parsers;

public class CallbackResponseParser : ResponseParser<TelegramUserMessage, CallbackResponse>
{
    protected override bool CanParse(TelegramUserMessage message) => 
        message.UpdateData.Type == UpdateType.CallbackQuery;

    protected async override ITask<ParsingResult> ParseResponseAsync(
        TelegramUserMessage message, CancellationToken token = default)
    {
        return ParsingResult.Ok(new CallbackResponse(message.UpdateData.CallbackQuery!.Data!));
        // return new CallbackResponse(message.UpdateData.CallbackQuery!.Data!);
    }
}