using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using PrettyBots.Storages.Abstraction;
using PrettyBots.Storages.Abstraction.Exceptions;
using PrettyBots.Storages.Abstraction.Model;

namespace PrettyBots.Storages.Json;

/// <summary>
/// Uses JSON file to store the data about user interaction.
/// </summary>
public class JsonStorageProvider<TUser> : IStorageProvider
    where TUser : class, IUser, new()
{
    public const string FILE_NAME = "users.json"; 
    
    protected ConcurrentDictionary<long, TUser>? Users { get; private set; }
    public string FilePath { get; protected set; } = Path.Combine(
        System.Environment.CurrentDirectory, FILE_NAME);

    private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
    private readonly ILogger<JsonStorageProvider<TUser>>? _logger;
    
    public JsonStorageProvider()
    {
        
    }
    
    public JsonStorageProvider(ILogger<JsonStorageProvider<TUser>> logger)
    {
        _logger = logger;
    }

    [MemberNotNullWhen(true, nameof(Users))]
    public bool Initialized { get; private set; } = false;

    [MemberNotNull(nameof(Users))]
    public async Task InitAsync(CancellationToken token = default)
    {
        if (Initialized) {
            return;
        }

        await _lock.WaitAsync(token).ConfigureAwait(false);
        try {
            if (!File.Exists(FilePath)) {
                File.Create(FilePath).Close();
            } else {
                string content = await File.ReadAllTextAsync(FilePath, token).ConfigureAwait(false);
                Users = JsonConvert.DeserializeObject<ConcurrentDictionary<long, TUser>>(content);
            }
        } catch (Exception e) {
            _logger?.LogError(e, "Error retrieving information from users file [{path}]",
                FilePath);
        } finally {
            Users ??= new ConcurrentDictionary<long, TUser>();
            _lock.Release();
            Initialized = true;
        }
    }

    public async Task StoreInteractionIdAsync(long userId, uint interactionId, 
        CancellationToken token = default)
    {
        EnsureInitialized();
        await _lock.WaitAsync(token).ConfigureAwait(false);
        try {
            if (Users.TryGetValue(userId, out TUser? user)) {
                user.CurrentInteractionId = interactionId;
            } else {
                TUser entity = new TUser { TelegramUserId = userId, CurrentInteractionId = interactionId };
                Users.TryAdd(userId, entity);
            }

            await using (StreamWriter writer = new StreamWriter(new FileStream(FilePath, FileMode.Truncate))) {
                await writer.WriteAsync(JsonConvert.SerializeObject(Users)).ConfigureAwait(false);
            }
        } catch (Exception e) {
            throw new StorageOperationException(StorageOpType.Write, e);
        } finally {
            _lock.Release();
        }
    }

    public Task<uint?> RetrieveInteractionIdAsync(long userId, 
        CancellationToken token = default)
    {
        EnsureInitialized();
        return Users.TryGetValue(userId, out TUser? user) 
            ? Task.FromResult((uint?)user.CurrentInteractionId) 
            : Task.FromResult<uint?>(null);
    }

    public Task<IUser?> RetrieveUserDataAsync(long userId, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task StoreInteractionDataAsync(long userId, string? data, CancellationToken token = default) { throw new NotImplementedException(); }

    public async Task SaveChangesAsync(CancellationToken token = default)
    {
        EnsureInitialized();
        await _lock.WaitAsync(token).ConfigureAwait(false);
        try {
            await using (StreamWriter writer = new StreamWriter(new FileStream(FilePath, FileMode.Truncate))) {
                await writer.WriteAsync(JsonConvert.SerializeObject(Users)).ConfigureAwait(false);
            }
        } catch (Exception e) {
            throw new StorageOperationException(StorageOpType.Read, e);
        } finally {
            _lock.Release();
        }
    }

    [MemberNotNull(nameof(Users))]
    protected void EnsureInitialized()
    {
        if (!Initialized) {
            throw new StorageNotInitializedException();
        }
    }

    public void Dispose()
    {
        _lock.Dispose();
        GC.SuppressFinalize(this);
    }
}