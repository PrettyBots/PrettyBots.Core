using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using PrettyBots.Environment;
using PrettyBots.Interactions.Abstraction;
using PrettyBots.Interactions.Abstraction.Services;
using PrettyBots.Interactions.Utilities.DependencyInjection;

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
    
    private ILogger<InteractionService<TMessage>> _logger;
    
    public InteractionService()
    {
        InternalInit();
    }
    
    [UsedImplicitly]
    public InteractionService(ILogger<InteractionService<TMessage>> logger, IEntitiesLoader loader, 
        ILoadedEntitiesRegistry registry, IConfigurationService config)
    {
        InternalInit(logger, loader, registry, config);
    }

    [MemberNotNull(nameof(Loader))]
    [MemberNotNull(nameof(Registry))]
    [MemberNotNull(nameof(Config))]
    [MemberNotNull(nameof(_logger))]
    private void InternalInit(ILogger<InteractionService<TMessage>>? logger = null,
        IEntitiesLoader? loader = null, ILoadedEntitiesRegistry? registry = null,
        IConfigurationService? config = null)
    {
        IServiceProvider provider = DefaultServiceProvider.BuildDefaultServiceProvider();

        _logger  = logger   ?? provider.GetRequiredService<ILogger<InteractionService<TMessage>>>();
        Config   = config   ?? provider.GetRequiredService<IConfigurationService>(); 
        Loader   = loader ?? provider.GetRequiredService<IEntitiesLoader>();
        Registry = registry ?? provider.GetRequiredService<ILoadedEntitiesRegistry>();
    }

    public Task HandleUserMessage(TMessage message, CancellationToken token = default)
    {
        return Task.CompletedTask;
    }
}