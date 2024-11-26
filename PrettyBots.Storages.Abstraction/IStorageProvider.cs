using PrettyBots.Storages.Abstraction.Model;

namespace PrettyBots.Storages.Abstraction;

/// <summary>
/// Service that controls the storage in which
/// the users and theirs current interaction ids are stored in.
/// </summary>
public interface IStorageProvider : IDisposable
{
    /// <summary>
    /// Sets or updates the current interaction id for the user.
    /// </summary>
    /// <param name="userId">User the current interaction id is set to.</param>
    /// <param name="interactionId">Value to be set.</param>
    /// <returns></returns>
    public Task StoreInteractionIdAsync(long userId, uint interactionId, 
        CancellationToken token = default);

    /// <summary>
    /// Fetches the current interaction id for the user.
    /// </summary>
    /// <param name="userId">User for which the current interaction should be fetched.</param>
    /// <returns>
    /// Current interaction id for the user or null,
    /// if the value hasn't been set for this user yet.
    /// </returns>
    public Task<uint?> RetrieveInteractionIdAsync(long userId, CancellationToken token = default);

    public Task<IUser?> RetrieveUserDataAsync(long userId, CancellationToken token = default);

    public Task StoreInteractionDataAsync(long userId, string? data, CancellationToken token = default);
}