using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using PrettyBots.Environment;
using PrettyBots.Interactions.Abstraction;
using PrettyBots.Interactions.Abstraction.Services;
using PrettyBots.Interactions.Services;
using PrettyBots.Storages.Abstraction;
using PrettyBots.Storages.Json;

namespace PrettyBots.Interactions.Utilities.Extensions;

public static class DIExtensions
{
   public static IServiceCollection AddInteractionFramework<TEnvironment, TUserMessage, TInteractionService>(this IServiceCollection collection) 
      where TEnvironment: class, IEnvironment
      where TUserMessage: class, IUserMessage
      where TInteractionService: class, IInteractionService, IMessageHandler<TUserMessage>
   {
      return collection
         .AddSingleton<IEnvironment, TEnvironment>()
         .AddSingleton<IEntitiesLoader, EntitiesLoader>()
         .AddSingleton<IConfigurationService, ConfigurationService>()
         .AddSingleton<ILoadedEntitiesRegistry, LoadedEntitiesRegistry>()
         .AddSingleton<IStorageProvider, JsonStorageProvider<BasicUser>>()
         .AddSingleton<IInteractionService, TInteractionService>()
         .AddSingleton(typeof(IMessageHandler<TUserMessage>),
            (sp) => sp.GetRequiredService<IInteractionService>());
   }
}