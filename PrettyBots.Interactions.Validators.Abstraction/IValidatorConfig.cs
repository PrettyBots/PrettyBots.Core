using PrettyBots.Environment;

namespace PrettyBots.Interactions.Validators.Abstraction;

/// <summary>
/// Configs the interaction response dependent on the factory.
/// </summary>
public interface IValidatorConfig { }

public interface IValidatorConfig<in TResponse> : IValidatorConfig
    where TResponse : IUserResponse { }

