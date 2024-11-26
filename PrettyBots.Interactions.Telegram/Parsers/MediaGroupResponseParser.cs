using System.Collections.Concurrent;

using MorseCode.ITask;

using PrettyBots.Environment.Model;
using PrettyBots.Environment.Parsers;
using PrettyBots.Environment.Parsers.Model;
using PrettyBots.Environment.Responses;
using PrettyBots.Environment.Responses.Media;
using PrettyBots.Environment.Responses.TextBased;
using PrettyBots.Environment.Telegram;

using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PrettyBots.Interactions.Telegram.Parsers;

public class MediaGroupResponseParser : ResponseParser<TelegramUserMessage, MediaGroupResponse>
{
    protected override bool CanParse(TelegramUserMessage message) =>
        message.UpdateData.Message?.MediaGroupId is not null;

    protected async override ITask<ParsingResult> ParseResponseAsync(TelegramUserMessage message, CancellationToken token = default)
    {
        return ParsingResult.Ok(new MediaGroupResponse() { MediaEntities = message.MediaEntities! });
    }
}
