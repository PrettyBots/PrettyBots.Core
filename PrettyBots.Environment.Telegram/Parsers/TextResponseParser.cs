using MorseCode.ITask;

using PrettyBots.Environment.Parsers;
using PrettyBots.Environment.Parsers.Model;
using PrettyBots.Environment.Responses.TextBased;

using Telegram.Bot.Types.Enums;

namespace PrettyBots.Environment.Telegram.Parsers;

public class TextResponseParser : ResponseParser<TelegramUserMessage, TextResponse>
{
    protected override bool CanParse(TelegramUserMessage message) => 
        message.UpdateData.Type == UpdateType.Message && message.UpdateData.Message!.Text is not null;

    protected override async ITask<ParsingResult> ParseResponseAsync(
        TelegramUserMessage message, CancellationToken token = default)
    {
        return ParsingResult.Ok(new TextResponse(message.UpdateData.Message!.Text!));
        //return new TextResponse(message.UpdateData.Message!.Text!);
    } 
}