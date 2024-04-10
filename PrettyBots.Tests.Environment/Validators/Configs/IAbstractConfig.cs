using PrettyBots.Interactions.Validators.Abstraction;
using PrettyBots.Tests.Environment.Responses;

namespace PrettyBots.Tests.Environment.Validators.Configs;

public interface IAbstractConfig<in TResponse> : IValidatorConfig<TResponse>
    where TResponse : IAbstractResponse
{
    public string TestParameter { get; }
}