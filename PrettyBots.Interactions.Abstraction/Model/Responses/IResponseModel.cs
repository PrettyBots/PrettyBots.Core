﻿using PrettyBots.Environment;

namespace PrettyBots.Interactions.Abstraction.Model.Responses;

/// <summary>
/// Describes the possible response for a specific interaction.
/// If the response is received, contains the data of the response.
/// </summary>
public interface IResponseModel
{
    /// <summary>
    /// Is used to identify the response for the interaction.
    /// </summary>
    public string Key { get; }
    
    /// <summary>
    /// Type of the response this model will produce.
    /// Should implement <see cref="IUserResponse"/> and should be a non-abstract class.
    /// </summary>
    public Type ResponseType { get; }
    
    /// <summary>
    /// Specifies the type of the parser that will be used to process the response.
    /// Is loaded when the response model is processed by the interaction service.
    /// </summary>
    public Type? ResponseParserType { get; set; }
}