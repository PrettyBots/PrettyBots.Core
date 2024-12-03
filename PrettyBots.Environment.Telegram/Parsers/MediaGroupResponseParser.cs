using MorseCode.ITask;

using PrettyBots.Environment.Parsers;
using PrettyBots.Environment.Parsers.Model;
using PrettyBots.Environment.Responses.Media;

namespace PrettyBots.Environment.Telegram.Parsers;

public class MediaGroupResponseParser : ResponseParser<TelegramUserMessage, MediaGroupResponse>
{
    protected override bool CanParse(TelegramUserMessage message) =>
        message.UpdateData.Message?.MediaGroupId is not null;

    protected async override ITask<ParsingResult> ParseResponseAsync(TelegramUserMessage message, CancellationToken token = default)
    {
        return ParsingResult.Ok(new MediaGroupResponse() { MediaEntities = message.MediaEntities! });
    }
}
