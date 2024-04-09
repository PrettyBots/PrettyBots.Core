using PrettyBots.Environment.Parsers;

namespace PrettyBots.Environment;

/// <summary>
/// Define the environment-based configurations and serves as
/// the determinant of the environment the interaction service operates in. 
/// </summary>
public interface IEnvironment
{
    /// <summary>
    /// Type of the message that implements <see cref="IUserMessage"/>
    /// and will be handled by the interaction service. This type should be declared
    /// by non-abstract public class to avoid startup critical errors.
    /// </summary>
    /// <remarks>
    /// All types of <see cref="ResponseParser{TMessage,TResponse}"/> would need to have
    /// that type as the type parameter in order to be detected and/or loaded.
    /// </remarks>
    public Type MessageType { get; }
}