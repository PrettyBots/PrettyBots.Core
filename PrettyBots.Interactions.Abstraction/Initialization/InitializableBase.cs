namespace PrettyBots.Interactions.Abstraction.Initialization;

/// <summary>
/// Base interface to conveniently use <see cref="IRequireInitialization"/>
/// interface.
/// Declares its own initialization method, after which the property
/// <see cref="Initialized"/> will be set to true.
/// </summary>
public abstract class InitializableBase : IRequireInitialization 
{
    public bool Initialized { get; private set; }
    
    public async Task InitAsync(CancellationToken token = default)
    {
        try {
            await InitializeAsync(token).ConfigureAwait(false);
        } finally {
            Initialized = true;
        }
    }

    /// <summary>
    /// Method in which initialization is declared.
    /// After the execution, whether or not it resulted in a error,
    /// <see cref="Initialized"/> will be set to true.
    /// </summary>
    protected abstract Task InitializeAsync(CancellationToken token = default);
}