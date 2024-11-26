using PrettyBots.Environment.Responses;

namespace PrettyBots.Attributes.Responses.Basic;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class MessageResponseAttribute : BasicResponseAttribute
{
    public MessageResponseAttribute(string key) : base(key, typeof(MessageResponse))
    {
    }
}