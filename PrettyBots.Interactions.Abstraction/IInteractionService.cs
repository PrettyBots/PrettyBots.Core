using PrettyBots.Interactions.Abstraction.Services;

namespace PrettyBots.Interactions.Abstraction;

/// <summary>
/// Service that handles interactions between the user and the bot.
/// </summary>
public interface IInteractionService
{
    public IEntitiesLoader Loader { get; }
    
    public ILoadedEntitiesRegistry Registry { get; } 
    
    public IConfigurationService Config { get; }
}
