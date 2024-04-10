using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using PrettyBots.Environment;
using PrettyBots.Interactions.Abstraction.Services;
using PrettyBots.Interactions.Services;

namespace PrettyBots.Interactions.Utilities.DependencyInjection;

public static class DefaultServiceProvider
{
    public static IServiceProvider BuildDefaultServiceProvider(IEnvironment environment)
    {
        ServiceCollection collection = new ServiceCollection();
        collection.AddSingleton(environment);
        collection.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
        collection.AddSingleton<IEntitiesLoader, EntitiesLoader>(); 
        collection.AddSingleton<IConfigurationService, ConfigurationService>();
        collection.AddSingleton<ILoadedEntitiesRegistry, LoadedEntitiesRegistry>();

        return collection.BuildServiceProvider();
    }
}