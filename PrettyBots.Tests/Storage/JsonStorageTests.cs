using PrettyBots.Storages.Abstraction.Exceptions;
using PrettyBots.Storages.Json;
using PrettyBots.Tests.Environment.Storage;

namespace PrettyBots.Tests.Storage;

[TestFixture]
[TestOf(typeof(JsonStorageProvider<>))]
public class JsonStorageTests
{
    private const string TEST_PARAM_VALUE = "something";
    
    [Test]
    public void TestNotInitializedExceptions_NoSP()
    {
        LocalJsonStorage storage = new LocalJsonStorage();

        Assert.ThrowsAsync<StorageNotInitializedException>(async () => {
            await storage.RetrieveInteractionIdAsync(0);
        });
        
        Assert.ThrowsAsync<StorageNotInitializedException>(async () => {
            await storage.StoreInteractionIdAsync(0, 0);
        });
    }

    [Test]
    public async Task TestBasicRW_NoSP()
    {
        using LocalJsonStorage storage = new LocalJsonStorage();
        File.Delete(storage.FilePath);
        
        await storage.InitAsync();
        await storage.StoreInteractionIdAsync(0, 12U);
        await storage.StoreInteractionIdAsync(0, 15U);
        uint? storedId = await storage.RetrieveInteractionIdAsync(0);
        Assert.That(storedId, Is.EqualTo(15U));
    }

    [Test]
    public async Task TestSessionsPersistence_NoSP()
    {
        using (LocalJsonStorage storage = new LocalJsonStorage()) {
            File.Delete(storage.FilePath);

            await storage.InitAsync();
            await storage.StoreInteractionIdAsync(1L, 17U);
            await storage.StoreInteractionIdAsync(2L, 12U);

            TestUser secondUser = storage.UserModels
                .First(u => u.UserId == 2L);
            secondUser.Test = TEST_PARAM_VALUE;
            await storage.SaveChangesAsync();
            
            await storage.StoreInteractionIdAsync(2L, 19U);
            await storage.StoreInteractionIdAsync(3L, 12U);
        }

        using (LocalJsonStorage newSessionStorage = new LocalJsonStorage()) {
            await newSessionStorage.InitAsync();
            
            Assert.That(newSessionStorage.UserModels, Has.Count.EqualTo(3));

            uint? firstUserIId = await newSessionStorage.RetrieveInteractionIdAsync(1L);
            uint? thirdUserIId = await newSessionStorage.RetrieveInteractionIdAsync(3L);
            
            Assert.That(firstUserIId, Is.EqualTo(17U));
            Assert.That(thirdUserIId, Is.EqualTo(12U));

            TestUser secondUser = newSessionStorage.UserModels.ElementAt(1);
            Assert.That(secondUser.UserId, Is.EqualTo(2L));
            Assert.That(secondUser.Test, Is.EqualTo(TEST_PARAM_VALUE));
            Assert.That(secondUser.CurrentInteractionId, Is.EqualTo(19U));
        }
    }
}