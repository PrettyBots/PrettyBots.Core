namespace PrettyBots.Storages.Abstraction.Model;

/// <summary>
/// Basic user model that contains user id and user's current interaction id.
/// </summary>
public interface IUser
{
    /// <summary>
    /// Specifies the id of the user of the bot,
    /// whether it is the user, or the chat.
    /// </summary>
    public long UserId { get; set; }
    
    /// <summary>
    /// Specifies the id of the interaction that is currently
    /// happening between the bot and the user, and will be used
    /// to determine the parsing and validation rules for the handling of
    /// the next user response.
    /// </summary>
    public uint CurrentInteractionId { get; set; }
}