using PrettyBots.Tests.Environment.Responses;
using PrettyBots.Validators.Abstraction;

namespace PrettyBots.Tests.Environment.Validators.Configs;

public interface IAbstractConfig<in TResponse> : IValidatorConfig<TResponse>
    where TResponse : IAbstractResponse
{
    public string TestParameter { get; }
}