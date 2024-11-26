using PrettyBots.Environment.Responses;
using PrettyBots.Environment.Responses.Media;

namespace PrettyBots.Attributes.Responses.Basic;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class MediaGroupResponseAttribute : BasicResponseAttribute
{
    public MediaGroupResponseAttribute(string key) : base(key, typeof(MediaGroupResponse))
    {
    }
}