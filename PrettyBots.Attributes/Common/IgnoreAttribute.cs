namespace PrettyBots.Attributes.Common;

/// <summary>
/// Parsers, validators, modules or handlers marked with this attribute will not be loaded.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class IgnoreAttribute : Attribute
{
    /// <summary>
    /// Reason to ignore this entity.
    /// </summary>
    public string? Reason { get; set; }
}