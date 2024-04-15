using Newtonsoft.Json;

using PrettyBots.Storages.Json;

namespace PrettyBots.Tests.Environment.Storage;

public class TestUser : BasicUser
{
    [JsonProperty("test")]
    public string? Test { get; set; }
}