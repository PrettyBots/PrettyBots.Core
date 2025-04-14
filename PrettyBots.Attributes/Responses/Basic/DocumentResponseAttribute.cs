using PrettyBots.Environment.Responses.Document;

namespace PrettyBots.Attributes.Responses.Basic;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class DocumentResponseAttribute : BasicResponseAttribute
{
    
    public DocumentResponseAttribute(string key) : base(key, typeof(DocumentResponse))
    {
    }
}