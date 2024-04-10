using PrettyBots.Interactions;
using PrettyBots.Interactions.Abstraction;
using PrettyBots.Interactions.Exceptions;
using PrettyBots.Tests.Environment;
using PrettyBots.Tests.Environment.Messages;

namespace PrettyBots.Tests;

[Order(0)]
public class InstanceTests
{
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    [TestOf(typeof(InteractionService<>))]
    public void TestValidInstance_NoSP()
    {
        IInteractionService service = new TestInteractionService();
    }

    [Test]
    public void TestIncorrectEnvironment_NoSP()
    {
        Assert.Throws<CriticalServiceException>(() => {
            IInteractionService service =
                new InteractionService<UnusedMessage>(TestEnvironment.Instance);
        });
    }
}