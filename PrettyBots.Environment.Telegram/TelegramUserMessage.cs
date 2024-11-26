using System.Diagnostics.CodeAnalysis;

using PrettyBots.Environment.Model;

using Telegram.Bot.Types;

namespace PrettyBots.Environment.Telegram;

public class TelegramUserMessage : IUserMessage
{
    public long UserId { get; }
    public Update UpdateData { get; }
    public List<IMediaEntity>? MediaEntities { get; }
    public IMediaEntity? MediaEntity { get; }

    public TelegramUserMessage(Update updateData, List<IMediaEntity>? mediaEntities = null,  IMediaEntity? mediaEntity = null)
    {
        UpdateData    = updateData;
        MediaEntity   = mediaEntity;
        MediaEntities = mediaEntities;
        UserId        = (updateData.Message?.From?.Id ?? updateData.CallbackQuery?.From.Id)!.Value;
    }
}