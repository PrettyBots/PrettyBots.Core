using PrettyBots.Environment.Responses.Media;

namespace PrettyBots.Attributes.Responses.Basic;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class MediaResponseAttribute : BasicResponseAttribute
{
    public MediaResponseAttribute(string key) : base(key, typeof(MediaResponse))
    {
    }
}