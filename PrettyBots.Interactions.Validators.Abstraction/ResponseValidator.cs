using PrettyBots.Environment;

namespace PrettyBots.Interactions.Validators.Abstraction;

/// <summary>
/// Type-safe implementation of the response validator.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public abstract class ResponseValidator<TResponse> : IResponseValidator<TResponse>
    where TResponse : IUserResponse
{
    protected abstract ValueTask<bool> ValidateAsync(TResponse response, IValidatorConfig config);

    public ValueTask<bool> ValidateResponseAsync(TResponse response, IValidatorConfig config) =>
        ValidateAsync(response, config);

    public ValueTask<bool> ValidateResponseAsync(IUserResponse response, IValidatorConfig config) =>
        ValidateResponseAsync((TResponse)response, config);
}