using System.Net.Mime;

namespace PrettyBots.Environment.Responses;

public class TextResponse : IUserResponse
{
    /// <summary>
    /// Text from the user message - <see cref="MediaTypeNames.Text"/>.
    /// </summary>
    public string Text { get; } = null!;

    public IEnvironment Environment { get; set; } = null!;
}
