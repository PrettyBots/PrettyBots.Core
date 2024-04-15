﻿using PrettyBots.Interactions.Abstraction.Model.Descriptors;
using PrettyBots.Utilities.Collections;

namespace PrettyBots.Interactions.Abstraction.Services;

/// <summary>
/// Service that is used to store the loaded entities,
/// takes responsibility of detecting duplicates
/// and throw respective errors when any.
/// </summary>
public interface ILoadedEntitiesRegistry
{
    /// <summary>
    /// Contains the list of loaded interactions,
    /// mapped to the interaction id.
    /// </summary>
    IReadOnlyDictionary<uint, InteractionInfo> Interactions { get; }
    
    /// <summary>
    /// Contains the list of loaded interaction modules,
    /// mapped to the interaction module type.
    /// </summary>
    IReadOnlyDictionary<Type, InteractionModuleInfo> InteractionModules { get; }
    
    /// <summary>
    /// Contains the list of loaded parsers for types,
    /// mapped to the response type.
    /// </summary>
    IReadOnlyDictionary<Type, DefaultEntityCollection<ResponseParserInfo>> 
        ResponseParsers { get; }
    
    IReadOnlyDictionary<Type, ResponseValidatorInfo> ResponseValidators { get; }
    
    void RegisterInteraction(InteractionInfo interactionInfo);
    
    void RegisterInteractionModule(InteractionModuleInfo moduleInfo);
    
    void RegisterResponseParser(ResponseParserInfo parserInfo);

    void RegisterValidator(ResponseValidatorInfo validatorInfo);
}