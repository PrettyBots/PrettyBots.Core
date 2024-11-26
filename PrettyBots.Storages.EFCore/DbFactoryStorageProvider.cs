using Microsoft.EntityFrameworkCore;

using PrettyBots.Storages.Abstraction;
using PrettyBots.Storages.Abstraction.Model;

namespace PrettyBots.Storages.EFCore;

public class DbFactoryStorageProvider<TContext, TUser> : IStorageProvider
    where TContext: InteractionsDataContext<TUser>
    where TUser: class, IUser
{
    private readonly IDbContextFactory<TContext> _contextFactory;

    public DbFactoryStorageProvider(IDbContextFactory<TContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task StoreInteractionIdAsync(long userId, uint interactionId, CancellationToken token = default)
    {
        await using (InteractionsDataContext<TUser> dataContext = await _contextFactory.CreateDbContextAsync(token).ConfigureAwait(false)) {
            await StorageOperationProcessor.StoreInteractionIdAsync(dataContext, userId, interactionId, token)
                .ConfigureAwait(false);
        }
    }
    
    public async Task<uint?> RetrieveInteractionIdAsync(long userId, CancellationToken token = default)
    {
        uint? interactionId;
        await using (InteractionsDataContext<TUser> dataContext = await _contextFactory.CreateDbContextAsync(token).ConfigureAwait(false)) {
            interactionId = await StorageOperationProcessor.RetrieveInteractionIdAsync(dataContext, userId, token)
                .ConfigureAwait(false);
        }

        return interactionId;
    }

    public async Task<IUser?> RetrieveUserDataAsync(long userId, CancellationToken token = default)
    {
        IUser? user;
        await using (InteractionsDataContext<TUser> dataContext = await _contextFactory.CreateDbContextAsync(token).ConfigureAwait(false)) {
            user = await StorageOperationProcessor.RetrieveUserDataAsync(dataContext, userId, token)
                .ConfigureAwait(false);
        }

        return user;
    }

    public async Task StoreInteractionDataAsync(long userId, string? data, CancellationToken token = default)
    {
        await using (InteractionsDataContext<TUser> dataContext = await _contextFactory.CreateDbContextAsync(token).ConfigureAwait(false)) {
            await StorageOperationProcessor.StoreInteractionDataAsync(dataContext, userId, data, token)
                .ConfigureAwait(false);
        }
    }

    public void Dispose()
    {
    }
}