using System.Net.Mime;

namespace PrettyBots.Environment.Responses.TextBased;

public class TextResponse : ITextBasedResponse
{
    /// <summary>
    /// Text from the user message - <see cref="MediaTypeNames.Text"/>.
    /// </summary>
    public string Text { get; } = null!;

    public IEnvironment Environment { get; set; } = null!;

    public TextResponse(string text)
    {
        Text = text;
    }
}
