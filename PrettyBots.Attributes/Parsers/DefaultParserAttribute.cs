using PrettyBots.Model.Responses;

namespace PrettyBots.Attributes.Parsers;

/// <summary>
/// Marks the parser as the default one for the type.
/// Default parsers are set by default for their types when the
/// <see cref="IResponseModel"/> doesn't specify the
/// <see cref="IResponseModel.ResponseParserType"/>
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class DefaultParserAttribute : Attribute
{   
}