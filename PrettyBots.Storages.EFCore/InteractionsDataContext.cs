using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;

using PrettyBots.Storages.Abstraction;
using PrettyBots.Storages.Abstraction.Exceptions;
using PrettyBots.Storages.Abstraction.Model;

namespace PrettyBots.Storages.EFCore;


public class InteractionsDataContext<TUser> : DbContext, IStorageProvider
    where TUser : class, IUser, new()
{
    [PublicAPI]
    protected DbSet<TUser> Users { get; set; } = null!;
    
    public async Task StoreInteractionIdAsync(long userId, uint interactionId,
        CancellationToken token = default)
    {
        try {
            TUser? user = await Users.FirstOrDefaultAsync(u =>
                u.UserId == userId, token).ConfigureAwait(false);

            if (user is not null) {
                user.CurrentInteractionId = interactionId;
            } else {
                TUser newUser = new TUser { 
                    UserId = userId, 
                    CurrentInteractionId = interactionId 
                };

                await Users.AddAsync(newUser, token).ConfigureAwait(false);
            }

            await SaveChangesAsync(token).ConfigureAwait(false);
        } catch (Exception e) {
            throw new StorageOperationException(StorageOpType.Write, e);
        }
    }

    public async Task<uint?> RetrieveInteractionIdAsync(long userId,
        CancellationToken token = default)
    {
        try {
            TUser? user = await Users.Where(u => u.UserId == userId)
                .FirstOrDefaultAsync(cancellationToken: token).ConfigureAwait(false);

            return user?.CurrentInteractionId;
        } catch (Exception e) {
            throw new StorageOperationException(StorageOpType.Read, e);
        }
    }
}