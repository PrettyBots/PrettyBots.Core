using Microsoft.EntityFrameworkCore;

using PrettyBots.Storages.Abstraction;
using PrettyBots.Storages.Abstraction.Model;

namespace PrettyBots.Storages.EFCore;

public class DbContextStorageProvider<TUser> : IStorageProvider
    where TUser: class, IUser
{
    private readonly InteractionsDataContext<TUser> _dataContext;

    public DbContextStorageProvider(InteractionsDataContext<TUser> dataContext)
    {
        _dataContext = dataContext;
    }

    public Task StoreInteractionIdAsync(long userId, uint interactionId, CancellationToken token = default) =>
        StorageOperationProcessor.StoreInteractionIdAsync(_dataContext, userId, interactionId, token);

    public Task<uint?> RetrieveInteractionIdAsync(long userId, CancellationToken token = default) =>
        StorageOperationProcessor.RetrieveInteractionIdAsync(_dataContext, userId, token);

    public Task<IUser?> RetrieveUserDataAsync(long userId, CancellationToken token = default) =>
        StorageOperationProcessor.RetrieveUserDataAsync(_dataContext, userId, token);

    public Task StoreInteractionDataAsync(long userId, string? data, CancellationToken token = default) =>
        StorageOperationProcessor.StoreInteractionDataAsync(_dataContext, userId, data, token);

    public void Dispose()
    {
        _dataContext.Dispose();
    }
}