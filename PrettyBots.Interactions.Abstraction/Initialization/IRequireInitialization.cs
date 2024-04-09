namespace PrettyBots.Interactions.Abstraction.Initialization;

/// <summary>
/// General interface for services that require initialization
/// before running other tasks.
/// </summary>
public interface IRequireInitialization
{
    /// <summary>
    /// Specifies whether this service was properly set-up
    /// using the <see cref="InitAsync"/> method.
    /// </summary>
    public bool Initialized { get; }
    
    /// <summary>
    /// Initializes that service.
    /// After the initiation process, the property <see cref="Initialized"/>
    /// should be set to true.
    /// </summary>
    public Task InitAsync(CancellationToken token = default);
}