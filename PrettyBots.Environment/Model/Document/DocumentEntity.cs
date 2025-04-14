namespace PrettyBots.Environment.Model.Document;

public class DocumentEntity : IDocumentEntity
{
    public string? Caption { get; init; }
    public string FileId { get; init; } = null!;
    public string FileUniqueId { get; set; }  = null!;
    public string? FileName { get; init; }
    public long? FileSize { get; init; }
    public string? MimeType { get; init; }
}