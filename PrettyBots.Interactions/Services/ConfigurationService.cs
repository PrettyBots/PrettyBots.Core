using PrettyBots.Interactions.Abstraction.Services;

namespace PrettyBots.Interactions.Services;

public class ConfigurationService : IConfigurationService
{
    public bool StrictLoadingModeEnabled { get; set; }
    
    
}