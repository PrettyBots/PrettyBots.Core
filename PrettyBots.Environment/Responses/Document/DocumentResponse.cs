using PrettyBots.Environment.Model.Document;

namespace PrettyBots.Environment.Responses.Document;

public class DocumentResponse : IUserResponse
{
    public IEnvironment Environment { get; set; } = null!;
    public IDocumentEntity DocumentEntity { get; set; } = null!;
}