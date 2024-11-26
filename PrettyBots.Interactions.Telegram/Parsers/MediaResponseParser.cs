using MorseCode.ITask;

using PrettyBots.Environment.Parsers;
using PrettyBots.Environment.Parsers.Model;
using PrettyBots.Environment.Responses.Media;
using PrettyBots.Environment.Telegram;

using Telegram.Bot.Types.Enums;

namespace PrettyBots.Interactions.Telegram.Parsers;

public class MediaResponseParser : ResponseParser<TelegramUserMessage, MediaResponse>
{
    protected override bool CanParse(TelegramUserMessage message) =>
        message.UpdateData.Message?.Type == MessageType.Photo || message.UpdateData.Message?.Type == MessageType.Video
                                                              || message.UpdateData.Message?.Type == MessageType.Audio
                                                              || message.UpdateData.Message?.Type == MessageType.Voice;

    protected async override ITask<ParsingResult> ParseResponseAsync(TelegramUserMessage message, CancellationToken token = default)
    {
        return ParsingResult.Ok(new MediaResponse() { MediaEntity = message.MediaEntity! });
    }
}