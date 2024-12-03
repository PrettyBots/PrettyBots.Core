using System.Globalization;

using MorseCode.ITask;

using PrettyBots.Environment.Parsers;
using PrettyBots.Environment.Parsers.Model;
using PrettyBots.Environment.Responses;

using Telegram.Bot.Types.Enums;

namespace PrettyBots.Environment.Telegram.Parsers;

public class DateResponseParser : ResponseParser<TelegramUserMessage, DateResponse>
{
    protected override bool CanParse(TelegramUserMessage message) => 
        message.UpdateData.Type == UpdateType.Message && message.UpdateData.Message!.Text is not null;

    protected override async ITask<ParsingResult> ParseResponseAsync(TelegramUserMessage message, 
        CancellationToken token = default)
    {
        return DateTime.TryParse(message.UpdateData.Message!.Text!, CultureInfo.GetCultureInfo("ru-RU"),
            DateTimeStyles.None, out DateTime date) ? 
            ParsingResult.Ok(new DateResponse(date)) : 
            ParsingResult.Error(ParsingErrorType.IncorrectDateString, "");
    }
}