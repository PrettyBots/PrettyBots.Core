using Newtonsoft.Json;

using PrettyBots.Storages.Abstraction.Model;

namespace PrettyBots.Storages.Json;

/// <summary>
/// Basic user model implementation that contains only necessary fields.
/// </summary>
public class BasicUser : IUser
{
    [JsonProperty("userId")]
    public long UserId { get; set; } = default!;
    
    [JsonProperty("currentInteractionId")]
    public uint CurrentInteractionId { get; set; } = default;
}