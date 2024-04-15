
using PrettyBots.Storages.Json;

namespace PrettyBots.Tests.Environment.Storage;

public class LocalJsonStorage : JsonStorageProvider<TestUser>
{
    public ICollection<TestUser> UserModels
    {
        get {
            EnsureInitialized();
            return Users.Values;
        }
    }
}