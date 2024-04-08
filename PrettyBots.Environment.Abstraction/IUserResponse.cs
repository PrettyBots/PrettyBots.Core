namespace PrettyBots.Environment;

/// <summary>
/// Contains data about user response to an interaction.
/// </summary>
public interface IUserResponse
{
    /// <summary>
    /// Environment the response was created in.
    /// </summary>
    public IEnvironment Environment { get; set; }
}