using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using JetBrains.Annotations;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using PrettyBots.Environment;
using PrettyBots.Interactions.Abstraction;
using PrettyBots.Interactions.Abstraction.Services;
using PrettyBots.Interactions.Exceptions;
using PrettyBots.Interactions.Utilities.DependencyInjection;
using PrettyBots.Interactions.Validators;

namespace PrettyBots.Interactions;

public class InteractionService<TMessage> : IInteractionService, 
                                            IMessageHandler<TMessage>
    where TMessage : class, IUserMessage
{
    /// <inheritdoc />
    public IEntitiesLoader Loader { get; private set; }
    
    /// <inheritdoc />
    public ILoadedEntitiesRegistry Registry { get; private set; }

    public IConfigurationService Config { get; private set; }
    
    public IEnvironment Environment { get; private set; }
    
    private ILogger<InteractionService<TMessage>> _logger;
    
    public InteractionService(IEnvironment environment)
    {
        InternalInit(environment: environment);
    }
    
    [UsedImplicitly]
    public InteractionService(ILogger<InteractionService<TMessage>> logger, IEnvironment environment, 
        IEntitiesLoader loader, ILoadedEntitiesRegistry registry, IConfigurationService config)
    {
        InternalInit(environment, logger, loader, registry, config);
    }

    [MemberNotNull(nameof(Loader))]
    [MemberNotNull(nameof(Registry))]
    [MemberNotNull(nameof(Config))]
    [MemberNotNull(nameof(_logger))]
    [MemberNotNull(nameof(Environment))]
    private void InternalInit(IEnvironment environment, ILogger<InteractionService<TMessage>>? logger = null,
        IEntitiesLoader? loader = null, ILoadedEntitiesRegistry? registry = null, IConfigurationService? config = null)
    {
        IServiceProvider provider = DefaultServiceProvider
            .BuildDefaultServiceProvider(environment);

        if (!typeof(TMessage).IsEquivalentTo(environment.MessageType)) {
            throw new CriticalServiceException("The interaction service was made to "           + 
                $"handle messages of the {typeof(TMessage)} type, but environment has set the " +
                $"message type to {environment.MessageType}");
        }
        
        Environment = environment;
        _logger     = logger   ?? provider.GetRequiredService<ILogger<InteractionService<TMessage>>>();
        Config      = config   ?? provider.GetRequiredService<IConfigurationService>(); 
        Loader      = loader   ?? provider.GetRequiredService<IEntitiesLoader>();
        Registry    = registry ?? provider.GetRequiredService<ILoadedEntitiesRegistry>();

        bool strictTemp = Config.StrictLoadingModeEnabled;
        Config.StrictLoadingModeEnabled = true;
        Loader.LoadResponseValidators(Assembly.GetAssembly(typeof(RichTextResponseValidator))!);
        Config.StrictLoadingModeEnabled = strictTemp;
    }

    public Task HandleUserMessage(TMessage message, CancellationToken token = default)
    {
        return Task.CompletedTask;
    }
}