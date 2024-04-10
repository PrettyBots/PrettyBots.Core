using PrettyBots.Tests.Environment.Responses;

namespace PrettyBots.Tests.Environment.Validators.Configs;

public class AbstractConfigImpl : IAbstractConfig<IAbstractResponse>
{
    public string TestParameter { get; set; }

    public AbstractConfigImpl(string testParameter)
    {
        TestParameter = testParameter;
    }
}