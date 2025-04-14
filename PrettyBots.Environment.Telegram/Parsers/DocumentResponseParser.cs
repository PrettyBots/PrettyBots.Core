using MorseCode.ITask;

using PrettyBots.Environment.Model.Document;
using PrettyBots.Environment.Model.Media;
using PrettyBots.Environment.Parsers;
using PrettyBots.Environment.Parsers.Model;
using PrettyBots.Environment.Responses.Document;

using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PrettyBots.Environment.Telegram.Parsers;

public class DocumentResponseParser : ResponseParser<TelegramUserMessage, DocumentResponse>
{
    protected override bool CanParse(TelegramUserMessage message) =>
        message.UpdateData.Message?.Type == MessageType.Document;

    protected async override ITask<ParsingResult> ParseResponseAsync(TelegramUserMessage message, CancellationToken token = default)
    {
        Message response = message.UpdateData.Message!;
        IDocumentEntity documentEntity;

        if (response.Document!.MimeType?.Contains("image") ?? false) {
            documentEntity = new PhotoDocumentEntity {
                Caption = response.Caption,
                FileName     = response.Document.FileName,
                FileSize     = response.Document.FileSize,
                FileId       = response.Document.FileId,
                FileUniqueId = response.Document.FileUniqueId,
                Size = new PhotoEntitySize {
                    Height   = response.Document.Thumbnail!.Height,
                    Width    = response.Document.Thumbnail!.Width,
                    FileId   = response.Document.Thumbnail!.FileId,
                    FileSize = response.Document.Thumbnail!.FileSize,
                },
                MimeType = response.Document.MimeType,
            };
        } else {
            documentEntity = new DocumentEntity {
                Caption = response.Caption,
                FileName     = response.Document.FileName,
                FileSize     = response.Document.FileSize,
                FileId       = response.Document.FileId,
                FileUniqueId = response.Document.FileUniqueId,
                MimeType     = response.Document.MimeType,
            };
        }
        
        return ParsingResult.Ok(new DocumentResponse { DocumentEntity = documentEntity });
    }
}