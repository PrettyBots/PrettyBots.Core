namespace PrettyBots.Environment.Model.Document;

public interface IDocumentEntity
{
    string? Caption { get; init; }
    string FileId { get; init; }
    string FileUniqueId { get; set; }
    string? FileName { get; init; }
    long? FileSize { get; init; }
    string? MimeType { get; init; }
}