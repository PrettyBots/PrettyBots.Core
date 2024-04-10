using Microsoft.Extensions.Logging;

using PrettyBots.Interactions;
using PrettyBots.Interactions.Abstraction.Services;
using PrettyBots.Tests.Environment.Messages;

namespace PrettyBots.Tests.Environment;

public class TestInteractionService : InteractionService<TestMessage>
{
    public TestInteractionService() : base(TestEnvironment.Instance)
    {
    }

    public TestInteractionService(ILogger<InteractionService<TestMessage>> logger, IEntitiesLoader loader, ILoadedEntitiesRegistry registry, IConfigurationService config) 
        : base(logger, TestEnvironment.Instance, loader, registry, config)
    {
    }
}