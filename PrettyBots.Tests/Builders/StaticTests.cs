using PrettyBots.Interactions.Builders;

namespace PrettyBots.Tests.Builders;


[TestFixture]
public class StaticTests
{
    private enum InvalidInteraction : sbyte
    {
        Test,
    }

    [Test]
    public void TestEnumInteractionBuilder()
    {
        Exception? exception = null;
        try {
            InteractionBuilder<InvalidInteraction>
                .WithId(InvalidInteraction.Test);
        } catch (ArgumentException e) {
            exception = e;
        }
        
        Assert.That(exception, Is.Not.Null);
    }
}