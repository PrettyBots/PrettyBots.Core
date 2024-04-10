using System.Reflection;

using PrettyBots.Interactions.Abstraction;
using PrettyBots.Tests.Environment;

namespace PrettyBots.Tests.Loading;

[Ignore("This is the abstract class for all the loading tests")]
public abstract class BaseLoadingTests
{
    protected Assembly EnvironmentAssembly = null!;
    protected IInteractionService InteractionService = null!;
    
    [SetUp]
    public void Setup()
    {
        InteractionService = new TestInteractionService();
        EnvironmentAssembly 
            = Assembly.GetAssembly(typeof(TestEnvironment)) 
              ?? throw new InvalidOperationException("Test environment assembly was not found");
    }
}