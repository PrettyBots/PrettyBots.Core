using Microsoft.EntityFrameworkCore;

using PrettyBots.Storages.Abstraction;
using PrettyBots.Storages.Abstraction.Exceptions;
using PrettyBots.Storages.Abstraction.Model;

namespace PrettyBots.Storages.EFCore;

public class StorageOperationProcessor
{
    
    public static async Task StoreInteractionIdAsync<TUser>(InteractionsDataContext<TUser> dataContext, long userId, uint interactionId,
        CancellationToken token = default)
        where TUser : class, IUser
    {
        // try {
        //     TUser? user = await dataContext.Users.FirstOrDefaultAsync(u =>
        //         u.TelegramUserId == userId, token).ConfigureAwait(false);
        //
        //     if (user is not null) {
        //         user.CurrentInteractionId = interactionId;
        //     } else {
        //         throw new StorageOperationException(StorageOpType.Write, new Exception());
        //     }
        //
        //     await dataContext.SaveChangesAsync(token).ConfigureAwait(false);
        // } catch (Exception e) {
        //     throw new StorageOperationException(StorageOpType.Write, e);
        // }
        
        try {
            int affected = await dataContext.Users.Where(u => u.TelegramUserId == userId)
                                            .ExecuteUpdateAsync(
                                                calls => calls.SetProperty(u => u.CurrentInteractionId, interactionId),
                                                cancellationToken: token).ConfigureAwait(false);
            if (affected == 0) {
                throw new StorageOperationException(StorageOpType.Write, new Exception());
            }

        } catch (Exception e) {
            throw new StorageOperationException(StorageOpType.Write, e);
        }
    }

    public static async Task<uint?> RetrieveInteractionIdAsync<TUser>(InteractionsDataContext<TUser> dataContext, long userId,
        CancellationToken token = default)
        where TUser : class, IUser

    {
        try {
            TUser? user = await dataContext.Users.FirstOrDefaultAsync(u => u.TelegramUserId == userId, cancellationToken: token).ConfigureAwait(false);

            return user?.CurrentInteractionId;
        } catch (Exception e) {
            throw new StorageOperationException(StorageOpType.Read, e);
        }
    }

    public static async Task<IUser?> RetrieveUserDataAsync<TUser>(InteractionsDataContext<TUser> dataContext, long userId, CancellationToken token = default)
        where TUser : class, IUser
    {
        try {
            TUser? user = await dataContext.Users.FirstOrDefaultAsync(u => u.TelegramUserId == userId, cancellationToken: token).ConfigureAwait(false);

            return user;
        } catch (Exception e) {
            throw new StorageOperationException(StorageOpType.Read, e);
        }
    }

    public static async Task StoreInteractionDataAsync<TUser>(InteractionsDataContext<TUser> dataContext, long userId, string? data, 
        CancellationToken token = default) 
        where TUser : class, IUser
    {
        try {
            int affected = await dataContext.Users.Where(u => u.TelegramUserId == userId)
                             .ExecuteUpdateAsync(calls => calls.SetProperty(u => u.InteractionData, data),
                                 cancellationToken: token).ConfigureAwait(false);
            if (affected == 0) {
                throw new StorageOperationException(StorageOpType.Write, new Exception());
            }

        } catch (Exception e) {
            throw new StorageOperationException(StorageOpType.Write, e);
        }
    }

}