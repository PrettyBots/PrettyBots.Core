namespace PrettyBots.Environment;

/// <summary>
/// Base interface for all the messages that can be handled
/// by the interactions service. 
/// </summary>
public interface IUserMessage
{
    /// <summary>
    /// Unique user identifier that specifies the sender,
    /// and allows to control interactions data for this user.
    /// </summary>
    public long UserId { get; }
}