using PrettyBots.Environment;
using PrettyBots.Validators.Abstraction.Model;

namespace PrettyBots.Validators.Abstraction;

/// <summary>
/// Type-safe implementation of the response validator.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public abstract class ResponseValidator<TResponse> : IResponseValidator<TResponse>
    where TResponse : IUserResponse
{
    protected abstract ValueTask<ValidationResult> ValidateAsync(TResponse response, IValidatorConfig config);

    public ValueTask<ValidationResult> ValidateResponseAsync(TResponse response, IValidatorConfig config) =>
        ValidateAsync(response, config);

    public ValueTask<ValidationResult> ValidateResponseAsync(IUserResponse response, IValidatorConfig config) =>
        ValidateResponseAsync((TResponse)response, config);
}