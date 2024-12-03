using System.Diagnostics.Contracts;
using System.Reflection;

using Microsoft.Extensions.Logging;

using PrettyBots.Attributes.Common;
using PrettyBots.Attributes.Parsers;
using PrettyBots.Attributes.Responses;
using PrettyBots.Attributes.Validators;
using PrettyBots.Environment;
using PrettyBots.Environment.Parsers;
using PrettyBots.Interactions.Abstraction.Model;
using PrettyBots.Interactions.Abstraction.Model.Context;
using PrettyBots.Interactions.Abstraction.Model.Delegates;
using PrettyBots.Interactions.Abstraction.Model.Descriptors;
using PrettyBots.Interactions.Abstraction.Model.Descriptors.Loading;
using PrettyBots.Interactions.Abstraction.Model.Descriptors.Loading.Abstraction;
using PrettyBots.Interactions.Abstraction.Model.Responses;
using PrettyBots.Interactions.Abstraction.Services;
using PrettyBots.Interactions.Attributes;
using PrettyBots.Interactions.Builders;
using PrettyBots.Interactions.Exceptions.Modules;
using PrettyBots.Interactions.Utilities.DependencyInjection;
using PrettyBots.Utilities.Extensions;
using PrettyBots.Validators.Abstraction;

namespace PrettyBots.Interactions.Services;

public class EntitiesLoader : IEntitiesLoader
{
    private static readonly TypeInfo _moduleInterfaceType = 
        typeof(IInteractionModule).GetTypeInfo();
    
    
    private readonly IEnvironment _environment;
    private readonly IConfigurationService _config;
    private readonly ILoadedEntitiesRegistry _entitiesRegistry;
    private readonly ILogger<IEntitiesLoader> _logger;
    
    public EntitiesLoader(ILoadedEntitiesRegistry entitiesRegistry, 
        ILogger<IEntitiesLoader> logger, IConfigurationService config,
        IEnvironment environment)
    {
        _logger           = logger;
        _config           = config;
        _environment      = environment;
        _entitiesRegistry = entitiesRegistry;
    }
    
    public MultipleLoadingResult<ModuleLoadingResult>
        LoadInteractionModules(Assembly interactionsAssembly,
            IServiceProvider? serviceProvider = null)
    {
        try {
            List<ModuleLoadingResult> results = new List<ModuleLoadingResult>();
            foreach (TypeInfo moduleType in SearchModules(interactionsAssembly)) {
                ModuleLoadingResult moduleLoadingResult = LoadInteractionModule(moduleType, serviceProvider);
                results.Add(moduleLoadingResult);
                
                if (!moduleLoadingResult.Loaded) {
                    continue;
                }

                InteractionModuleInfo moduleInfo = moduleLoadingResult.Info;
                _entitiesRegistry.RegisterInteractionModule(moduleInfo);
            }

            return MultipleLoadingResult<ModuleLoadingResult>.FromSuccess(results);
        } catch (Exception e) {
            if (_config.StrictLoadingModeEnabled) {
                throw;
            }
            
            return MultipleLoadingResult<ModuleLoadingResult>.FromFailure(e);
        }
    }

    public ModuleLoadingResult LoadInteractionModule<TModule>()
        where TModule : IInteractionModule
    {
        return LoadInteractionModule(typeof(TModule));
    }
    
    public ModuleLoadingResult LoadInteractionModule(Type moduleType, 
        IServiceProvider? serviceProvider = null)
    {
        serviceProvider ??= new EmptyServiceProvider();
        try {
            if (!IsValidModuleDefinition(moduleType)) {
                ModuleLoadingException exception = new ModuleLoadingException(moduleType,
                    $"Module definition should be a non-nested non-abstract public class, " +
                    $"that doesn't have generic parameters, but found class does not fit "  +
                    $"in these constrains");

                HandleSoftLoadingException(exception);
                return ModuleLoadingResult.FromFailure(moduleType.Name, exception);
            }

            IInteractionModule? instance = (IInteractionModule?)serviceProvider.GetService(moduleType);
            if (instance is null) {
                if (!moduleType.GetConstructors().Any(c => c.IsPublic && c.GetParameters().Length == 0)) {
                    ModuleLoadingException exception = new ModuleLoadingException(moduleType,
                        "Module should be either added to service provider's collection " +
                        "or should have a parameterless constructor in order to instantiate it");

                    HandleSoftLoadingException(exception);
                    return ModuleLoadingResult.FromFailure(moduleType.Name, exception);
                }

                try {
                    instance = (IInteractionModule)Activator.CreateInstance(moduleType)!;
                } catch (Exception e) {
                    ModuleLoadingException exception = new ModuleLoadingException(moduleType, e.Message);
                    HandleSoftLoadingException(exception);
                    return ModuleLoadingResult.FromFailure(moduleType.Name, exception);
                }
            }

            List<GenericLoadingResult<IInteraction>> interactionLoadingResults = new();
            foreach (IInteraction interaction in instance.DeclareInteractions()) {
                interactionLoadingResults.Add(LoadInteraction(interaction));
            }

            InteractionModuleInfo moduleInfo = new InteractionModuleInfo(
                moduleType, serviceProvider, instance);

            List<InteractionHandlerInfo> loadedHandlers = new();
            List<GenericLoadingResult<InteractionHandlerInfo>> handlerInfosBuildingResult = new();
            foreach (MethodInfo methodInfo in moduleInfo.Type.GetMethods()) {
                InteractionHandlerAttribute? handlerAttribute =
                    methodInfo.GetCustomAttribute<InteractionHandlerAttribute>();

                if (handlerAttribute is null) {
                    continue;
                }

                IEnumerable<ResponseBaseAttribute> responseAttributes =
                    methodInfo.GetCustomAttributes<ResponseBaseAttribute>().ToArray();

                if (responseAttributes.Any()) {
                    InteractionBuilder builder = InteractionBuilder.WithId(handlerAttribute.InteractionId);
                    foreach (ResponseBaseAttribute attribute in responseAttributes) {
                        builder.WithResponse(attribute.CreateModel());
                    }

                    GenericLoadingResult<IInteraction> interactionLoadingResult = LoadInteraction(builder.Build());
                    interactionLoadingResults.Add(interactionLoadingResult);
                }
                
                if (!_entitiesRegistry.Interactions.ContainsKey(handlerAttribute.InteractionId)) {
                    HandlerLoadingException exception = new HandlerLoadingException(moduleInfo.Type,
                        methodInfo, 
                        $"Interaction {handlerAttribute.InteractionId} was not registered " +
                        $"before the handler loading");

                    HandleSoftLoadingException(exception);
                    handlerInfosBuildingResult.Add(GenericLoadingResult<InteractionHandlerInfo>.FromFailure(
                        methodInfo.Name, exception));
                    continue;
                }
                
                InteractionInfo currentHandlerInteractionInfo =
                    _entitiesRegistry.Interactions[handlerAttribute.InteractionId];
                
                if (currentHandlerInteractionInfo.HandlerInfo is not null) {
                    HandlerLoadingException exception = new HandlerLoadingException(moduleInfo.Type,
                        methodInfo, $"The handler for the interaction {handlerAttribute.InteractionId} " +
                                    $"was already found");

                    HandleSoftLoadingException(exception);
                    handlerInfosBuildingResult.Add(GenericLoadingResult<InteractionHandlerInfo>.FromFailure(
                        methodInfo.Name, exception));
                    continue;
                }

                if (!IsValidHandlerDefinition(methodInfo)) {
                    HandlerLoadingException exception = new HandlerLoadingException(moduleInfo.Type,
                        methodInfo, 
                        "Handler definition should be a non-static, non-abstract "       +
                        "public method, that doesn't have generic parameters, but found method " +
                        "does not fit in these constraints");

                    HandleSoftLoadingException(exception);
                    handlerInfosBuildingResult.Add(GenericLoadingResult<InteractionHandlerInfo>.FromFailure(
                        methodInfo.Name, exception));
                    continue;
                }

                bool  isAsync;
                bool  isCancellable = false;
                bool  acceptsSpecificContext;
                Type? specificContextResponseType = null;
                
                if (methodInfo.ReturnType.IsEquivalentTo(typeof(void))) {
                    isAsync = false;
                } else if (methodInfo.ReturnType.IsEquivalentTo(typeof(Task))) {
                    isAsync = true;
                } else {
                    HandlerLoadingException exception = new HandlerLoadingException(moduleInfo.Type,
                        methodInfo, 
                        "Handler method should only return void or Task, if used asynchronously");

                    HandleSoftLoadingException(exception);
                    handlerInfosBuildingResult.Add(GenericLoadingResult<InteractionHandlerInfo>.FromFailure(
                        methodInfo.Name, exception));
                    continue;
                }

                ParameterInfo[] handlerParams = methodInfo.GetParameters();
                if (handlerParams.Length is 0 or > 2) {
                    HandlerLoadingException exception = new HandlerLoadingException(moduleInfo.Type,
                        methodInfo, 
                        "Handler method should have 1 or 2 parameters.");

                    HandleSoftLoadingException(exception);
                    handlerInfosBuildingResult.Add(GenericLoadingResult<InteractionHandlerInfo>.FromFailure(
                        methodInfo.Name, exception));
                    continue;
                }

                if (handlerParams.Length == 2) {
                    if (handlerParams[1].ParameterType.IsEquivalentTo(typeof(CancellationToken))) {
                        isCancellable = true;
                    } else {
                        HandlerLoadingException exception = new HandlerLoadingException(moduleInfo.Type,
                            methodInfo, 
                            "The second parameter of the handler method, if present, should " +
                            "be of the CancellationToken type");

                        HandleSoftLoadingException(exception);
                        handlerInfosBuildingResult.Add(GenericLoadingResult<InteractionHandlerInfo>.FromFailure(
                            methodInfo.Name, exception));
                        continue;
                    }
                }
                
                Type firstParamType = handlerParams[0].ParameterType;
                if (!firstParamType.IsGenericType || !firstParamType.GetGenericTypeDefinition()
                        .IsEquivalentTo(typeof(IInteractionContext<,,>))) {
                    HandlerLoadingException exception = new HandlerLoadingException(moduleInfo.Type,
                        methodInfo, "The first handler parameter should be of the " +
                                    "IInteractionContext<> type");

                    HandleSoftLoadingException(exception);
                    handlerInfosBuildingResult.Add(GenericLoadingResult<InteractionHandlerInfo>.FromFailure(methodInfo.Name, exception));
                    continue;
                }

                Type[] contextTypeArguments = firstParamType.GenericTypeArguments;
                if (!contextTypeArguments.First().IsEquivalentTo(_environment.MessageType)) {
                    HandlerLoadingException exception = new HandlerLoadingException(moduleInfo.Type,
                        methodInfo, "The first handler parameter was the "                                 +
                        $"interaction context for the messages with type {contextTypeArguments.First()}, " +
                        $"but the environment declared to use {_environment.MessageType} as "              +
                        $"the message type");

                    HandleSoftLoadingException(exception);
                    handlerInfosBuildingResult.Add(GenericLoadingResult<InteractionHandlerInfo>.FromFailure(methodInfo.Name, exception));
                    continue;
                }
                
                //TODO: check user type
                Type userType = contextTypeArguments[1];

                if (contextTypeArguments[2].IsEquivalentTo(typeof(IUserResponse))) {
                    acceptsSpecificContext = false;
                } else {
                    acceptsSpecificContext      = true;
                    specificContextResponseType = contextTypeArguments[2];
                }

                if (acceptsSpecificContext && (
                        currentHandlerInteractionInfo.Interaction.AvailableResponses.Count is 0 or > 1
                        || !currentHandlerInteractionInfo.Interaction.AvailableResponses[0]
                                                        .GetType()
                                                        .GenericTypeArguments[0]
                                                        .IsEquivalentTo(specificContextResponseType))) {
                    HandlerLoadingException exception = new HandlerLoadingException(moduleInfo.Type,
                        methodInfo, 
                        "If the handler specifies response type, when declaring the context as "         +
                        "its first argument, the interaction it is assigned to should have no more then " +
                        "one response with the type that matches declared specific type");

                    HandleSoftLoadingException(exception);
                    handlerInfosBuildingResult.Add(GenericLoadingResult<InteractionHandlerInfo>.FromFailure(methodInfo.Name, exception));
                    continue;
                }

                if (!isAsync && isCancellable) {
                    HandlerLoadingException exception = new HandlerLoadingException(moduleInfo.Type,
                        methodInfo, "Sync handlers can't contain cancellation token as the second parameter");

                    HandleSoftLoadingException(exception);
                    handlerInfosBuildingResult.Add(GenericLoadingResult<InteractionHandlerInfo>.FromFailure(methodInfo.Name, exception));
                    continue;
                }
                
                InteractionHandlerInfo handlerInfo = new InteractionHandlerInfo(
                    handlerAttribute.InteractionId, handlerAttribute.RunMode, methodInfo, moduleInfo,
                    _environment.MessageType, userType, acceptsSpecificContext, isAsync, isCancellable, 
                    specificContextResponseType);
                
                loadedHandlers.Add(handlerInfo);
                currentHandlerInteractionInfo.HandlerInfo = handlerInfo;
                handlerInfosBuildingResult.Add(GenericLoadingResult<InteractionHandlerInfo>.FromSuccess(handlerInfo.Name, handlerInfo));
            }
            
            if (loadedHandlers.Count == 0) {
                ModuleLoadingException exception = new ModuleLoadingException(moduleType,
                    $"Module {moduleType.FullName} does not have any interaction handlers");

                HandleSoftLoadingException(exception);
                return ModuleLoadingResult.FromFailure(moduleType.Name, exception, handlerInfosBuildingResult,
                    interactionLoadingResults);
            }

            moduleInfo.HandlerInfos.AddRange(loadedHandlers);
            return ModuleLoadingResult.FromSuccess(moduleType.Name, moduleInfo, handlerInfosBuildingResult,
                interactionLoadingResults);
        } catch (Exception e) {
            if (_config.StrictLoadingModeEnabled) {
                throw;
            }

            return ModuleLoadingResult.FromFailure(moduleType.Name, e);
        }
    }

    public GenericLoadingResult<IInteraction> LoadInteraction(IInteraction interaction)
    {
        try {
            List<Type>   responseTypes = new List<Type>();
            List<string> responseKeys  = new List<string>();
            foreach (IResponseModel response in interaction.AvailableResponses) {
                if (responseTypes.Any(t => t.IsEquivalentTo(response.ResponseType))) {
                    InteractionLoadingException exception = new InteractionLoadingException(interaction,
                        "Interaction should not have responses of the same type");

                    HandleSoftLoadingException(exception);
                    return GenericLoadingResult<IInteraction>.FromFailure($"{interaction.Id} - {response.Key}",
                        exception);
                }
                
                if (responseKeys.Any(k => k == response.Key)) {
                    InteractionLoadingException exception = new InteractionLoadingException(interaction,
                        "Interaction should not have responses with the same keys");

                    HandleSoftLoadingException(exception);
                    return GenericLoadingResult<IInteraction>.FromFailure($"{interaction.Id} - {response.Key}",
                        exception);
                }

                responseKeys.Add(response.Key);
                responseTypes.Add(response.ResponseType);
                
                if (response.ResponseParserType is null) {
                    if (_entitiesRegistry.ResponseParsers.ContainsKey(response.ResponseType)) {
                        response.ResponseParserType = _entitiesRegistry.ResponseParsers[response.ResponseType]
                                                                       .Default?.ParserType;
                    }

                    if (response.ResponseParserType is null) {
                        InteractionLoadingException exception = new InteractionLoadingException(interaction,
                            $"Interaction contains invalid response with the key {response.Key}, " +
                            $"that declared no parser type, but there are no registered parsers "  +
                            $"registered to handle response of that type");

                        HandleSoftLoadingException(exception);
                        return GenericLoadingResult<IInteraction>.FromFailure($"{interaction.Id} - {response.Key}",
                            exception);
                    }

                    _logger.LogInformation("The response with the key {responseKey} of the " +
                                           "interaction with id {interactionId} will use a parser of type {parserType} " +
                                           "that had been assigned to it by default", response.Key,
                        interaction.Id, response.ResponseParserType);
                } else if (!_entitiesRegistry.ResponseParsers.ContainsKey(response.ResponseType) ||
                           _entitiesRegistry.ResponseParsers[response.ResponseType]
                                            .All(p => p.ParserType != response.ResponseParserType)) {
                    InteractionLoadingException exception = new InteractionLoadingException(interaction,
                        $"Interaction contains invalid response with the key {response.Key}, "      +
                        $"that declared a parser type {response.ResponseParserType}, that has not " +
                        $"been registered");

                    HandleSoftLoadingException(exception);
                    return GenericLoadingResult<IInteraction>.FromFailure($"{interaction.Id} - {response.Key}",
                        exception);
                }

                if (response is IValidatableResponseModel vResponse && 
                    (vResponse.ResponseValidatorType is not null || vResponse.Validator is not null)) {
                    
                    Type validatorType = vResponse.ResponseValidatorType ?? vResponse.Validator!.GetType();
                    if (!_entitiesRegistry.ResponseValidators.TryGetValue(validatorType,
                            out ResponseValidatorInfo? info)) {
                        InteractionLoadingException exception = new InteractionLoadingException(interaction,
                            $"Interaction contains invalid response with the key {response.Key}, " +
                            $"that declared {vResponse.ResponseValidatorType} as a validator "     +
                            $"but this validator has not been registered");


                        return GenericLoadingResult<IInteraction>.FromFailure($"{interaction.Id} - {response.Key}",
                            exception);
                    }
                    
                    if (vResponse.ResponseValidatorType is not null) {
                        vResponse.Validator = info.CreateInstance();
                    }
                    
                    if (vResponse.Config is not null && info.AvailableConfigTypes
                            .All(c => !c.IsInstanceOfType(vResponse.Config))) {
                        InteractionLoadingException exception = new InteractionLoadingException(interaction,
                            $"Interaction contains invalid response with the key {response.Key}, "      +
                            $"that declared config with type {vResponse.Config.GetType()}, "            +
                            $"but the validator {vResponse.ResponseValidatorType} does not allow that " +
                            $"config type to be used");


                        return GenericLoadingResult<IInteraction>.FromFailure($"{interaction.Id} - {response.Key}",
                            exception);
                    }
                }
            }

            _entitiesRegistry.RegisterInteraction(new InteractionInfo(interaction, null));
            return GenericLoadingResult<IInteraction>.FromSuccess($"Interaction [{interaction.Id}]", interaction);
        } catch (Exception e) {
            HandleSoftLoadingException(e);
            return GenericLoadingResult<IInteraction>.FromFailure($"Interaction [{interaction.Id}]", e);
        }
    }

    public GenericMultipleLoadingResult<ResponseParserInfo>
        LoadResponseParsers(Assembly parsersAssembly, 
            IServiceProvider? serviceProvider = null)
    {
        serviceProvider ??= new EmptyServiceProvider();
        List<GenericLoadingResult<ResponseParserInfo>> results = new();
        foreach (TypeInfo typeInfo in parsersAssembly.DefinedTypes) {
            if (!typeof(IResponseParser<IUserResponse>).IsAssignableFrom(typeInfo) 
                || typeof(IResponseParser<>).IsEquivalentTo(typeInfo)) {
                continue;
            }
            
            results.Add(LoadResponseParser(typeInfo, serviceProvider));
        }

        return GenericMultipleLoadingResult<ResponseParserInfo>.FromSuccess(results);
    }
    
    public GenericLoadingResult<ResponseParserInfo> 
        LoadResponseParser<TMessage, TResponse, TParser>(IServiceProvider? serviceProvider = null)
            where TMessage : class, IUserMessage
            where TResponse : class, IUserResponse
            where TParser : ResponseParser<TMessage, TResponse>
    {
        return LoadResponseParser(typeof(TParser), serviceProvider);
    }
    
    private GenericLoadingResult<ResponseParserInfo>
        LoadResponseParser(Type parserType, IServiceProvider? serviceProvider = null)
    {
        serviceProvider ??= new EmptyServiceProvider();
        try {
            IgnoreAttribute? ignoreAttribute = parserType.GetCustomAttribute<IgnoreAttribute>();
            if (ignoreAttribute is not null) {
                _logger.LogInformation("Ignoring the parser {parserType}" 
                    + (ignoreAttribute.Reason is not null 
                        ? $" due to {ignoreAttribute.Reason}" 
                        : ""), 
                    parserType.FullName);
                return GenericLoadingResult<ResponseParserInfo>.FromFailure(parserType.Name, 
                    new ParserLoadingException(parserType, "Ignoring the parser due to the attribute is set"));
            }
            
            if (!IsValidParserDefinition(parserType)) {
                ParserLoadingException exception = new ParserLoadingException(parserType,
                    $"Parser definition should be a non-abstract public class, " +
                    $"but found class does not fit in these constrains");

                HandleSoftLoadingException(exception);
                return GenericLoadingResult<ResponseParserInfo>.FromFailure(parserType.Name,
                    exception);
            }

            Type[]? parserGenericArguments = parserType.GetParentGenericTypeArguments(typeof(ResponseParser<,>));
            if (parserGenericArguments is null) {
                ParserLoadingException exception = new ParserLoadingException(parserType,
                    $"Parser should inherit ResponseParser class in order to be loaded");

                HandleSoftLoadingException(exception);
                return GenericLoadingResult<ResponseParserInfo>.FromFailure(parserType.Name,
                    exception);
            }

            bool isDefault = parserType.GetCustomAttribute<DefaultParserAttribute>() is not null;
            
            Type messageType = parserGenericArguments[0];
            if (!messageType.IsEquivalentTo(_environment.MessageType)) {
                ParserLoadingException exception = new ParserLoadingException(parserType,
                    $"Attempt to load parser that is made for messages with type " +
                    $"{messageType}, but the environment of the service declares " +
                    $"{_environment.MessageType} as the message type");

                HandleSoftLoadingException(exception);
                return GenericLoadingResult<ResponseParserInfo>.FromFailure(parserType.Name,
                    exception);
            }
            
            Type responseType = parserGenericArguments[1];
            if (responseType is not { IsClass: true, IsAbstract: false }) {
                ParserLoadingException exception = new ParserLoadingException(parserType,
                    $"Type of the response in the parser definition should be an " +
                    $"instantiable class");

                HandleSoftLoadingException(exception);
                return GenericLoadingResult<ResponseParserInfo>.FromFailure(parserType.Name,
                    exception);
            }

            IResponseParser<IUserResponse>? instance = (IResponseParser<IUserResponse>?)
                serviceProvider.GetService(parserType);

            if (instance is null) {
                if (!parserType.GetConstructors().Any(c => c.IsPublic && c.GetParameters().Length == 0)) {
                    ParserLoadingException exception = new ParserLoadingException(parserType,
                        "The parser should be either added to service provider's collection " +
                        "or should have a parameterless constructor in order to instantiate it");

                    HandleSoftLoadingException(exception);
                    return GenericLoadingResult<ResponseParserInfo>.FromFailure(parserType.Name,
                        exception);
                }

                try {
                    instance = (IResponseParser<IUserResponse>)Activator.CreateInstance(parserType)!;
                } catch (Exception e) {
                    ParserLoadingException exception = new ParserLoadingException(parserType, e.Message);
                    HandleSoftLoadingException(exception);
                    return GenericLoadingResult<ResponseParserInfo>.FromFailure(parserType.Name,
                        exception);
                }
            }

            ResponseParserInfo parserInfo = new ResponseParserInfo(parserType, responseType, 
                isDefault, instance);
            _entitiesRegistry.RegisterResponseParser(parserInfo);
            return GenericLoadingResult<ResponseParserInfo>.FromSuccess(parserType.Name, parserInfo);
        } catch (Exception e) {
            if (_config.StrictLoadingModeEnabled) {
                throw;
            }
            
            return GenericLoadingResult<ResponseParserInfo>.FromFailure(parserType.Name, e);
        }
    }

    public GenericMultipleLoadingResult<ResponseValidatorInfo> LoadResponseValidators(
        Assembly validatorsAssembly, IServiceProvider? serviceProvider = null)
    {
        serviceProvider ??= new EmptyServiceProvider();
        List<GenericLoadingResult<ResponseValidatorInfo>> results = new();
        foreach (TypeInfo typeInfo in validatorsAssembly.DefinedTypes) {
            if (!typeInfo.IsAssignableTo(typeof(IResponseValidator)) 
                || typeof(IResponseValidator).IsEquivalentTo(typeInfo) 
                || typeof(IResponseValidator<>).IsEquivalentTo(typeInfo) 
                || typeof(ResponseValidator<>).IsEquivalentTo(typeInfo)) {
                continue;
            }
            
            results.Add(LoadResponseValidator(typeInfo, serviceProvider));
        }

        return GenericMultipleLoadingResult<ResponseValidatorInfo>.FromSuccess(results);
    }

    public GenericLoadingResult<ResponseValidatorInfo> LoadResponseValidator<TResponse, TValidator>(
        IServiceProvider? serviceProvider = null)
        where TResponse  : IUserResponse
        where TValidator : IResponseValidator<TResponse>
    {
        return LoadResponseValidator(typeof(TValidator), serviceProvider);
    }
    
    public GenericLoadingResult<ResponseValidatorInfo> LoadResponseValidator(Type validatorType,
        IServiceProvider? serviceProvider = null)
    {
        serviceProvider ??= new EmptyServiceProvider();

        try {
            IgnoreAttribute? ignoreAttribute = validatorType.GetCustomAttribute<IgnoreAttribute>();
            if (ignoreAttribute is not null) {
                _logger.LogInformation("Ignoring the validator {validatorType}" 
                    + (ignoreAttribute.Reason is not null 
                        ? $" due to {ignoreAttribute.Reason}" 
                        : ""), 
                    validatorType.FullName);
                return GenericLoadingResult<ResponseValidatorInfo>.FromFailure(validatorType.Name, 
                    new ValidatorLoadingException(validatorType, "Ignoring the validator due to the attribute is set"));
            }
            
            if (!validatorType.IsAssignableTo(typeof(IResponseValidator)) 
                    || typeof(IResponseValidator).IsEquivalentTo(validatorType) 
                    || typeof(IResponseValidator<>).IsEquivalentTo(validatorType) 
                    || typeof(ResponseValidator<>).IsEquivalentTo(validatorType)) {
                
                ValidatorLoadingException exception = new ValidatorLoadingException(validatorType,
                    "Attempt to load the type that doesn't implement IResponseValidator as a " +
                    "response validator or is one of library abstraction validators");

                HandleSoftLoadingException(exception);
                return GenericLoadingResult<ResponseValidatorInfo>.FromFailure(validatorType.Name, exception);
            }

            if (!IsValidValidatorDefinition(validatorType)) {
                ValidatorLoadingException exception = new ValidatorLoadingException(validatorType,
                    "Validator definition should be a non-abstract public class, " +
                    "but found class does not fit in these constrains");
            
                HandleSoftLoadingException(exception);
                return GenericLoadingResult<ResponseValidatorInfo>.FromFailure(validatorType.Name, exception);
            }
        
            bool useSP = serviceProvider.GetService(validatorType) is not null;
            if (!useSP) {
                if (!validatorType.GetConstructors().Any(c => c.IsPublic 
                                                              && c.GetParameters().Length == 0)) {
                    ValidatorLoadingException exception = new ValidatorLoadingException(validatorType,
                        "The validator should be either added to service provider's collection " +
                        "or should have a parameterless constructor in order to instantiate it");

                    HandleSoftLoadingException(exception);
                    return GenericLoadingResult<ResponseValidatorInfo>.FromFailure(validatorType.Name,
                        exception);
                }

                try {
                    Activator.CreateInstance(validatorType);
                } catch (Exception e) {
                    ValidatorLoadingException exception = new ValidatorLoadingException(validatorType, e.Message);
                    HandleSoftLoadingException(exception);
                    return GenericLoadingResult<ResponseValidatorInfo>.FromFailure(validatorType.Name,
                        exception);
                }
            }
        
            Type? responseType = validatorType
                .GetParentGenericTypeArguments(typeof(IResponseValidator<>))?[0];
            responseType ??= typeof(IUserResponse);

            List<Type> availableConfigTypes = new List<Type>();
            if (validatorType.GetCustomAttribute<ConfigurableWithAnyAttribute>() is not null) {
                availableConfigTypes.Add(typeof(IValidatorConfig<IUserResponse>));
            }

            if (validatorType.GetCustomAttribute<ConfigurableWithAnyOfMyTypeAttribute>() is not null) {
                availableConfigTypes.Add(typeof(IValidatorConfig<>).MakeGenericType(responseType));
            }

            foreach (ConfigurableWithAttribute configurableWith in validatorType
                         .GetCustomAttributes<ConfigurableWithAttribute>()) {

                Type configType         = configurableWith.ConfigType;
                Type? configResponseType = configType.GetParentGenericTypeArguments(typeof(IValidatorConfig<>))?[0];
                
                if (configResponseType is null) {
                    ValidatorLoadingException exception = new ValidatorLoadingException(validatorType,
                        "One of the ConfigurableWith attributes declared that this validator "             +
                        $"should accept the config of type {configType.Name} but this config type is not " +
                        "assignable to IValidatorConfig<IUserResponse> and thus, can't be used as "   +
                        "a valid configuration type");

                    HandleSoftLoadingException(exception);
                    return GenericLoadingResult<ResponseValidatorInfo>.FromFailure(validatorType.Name,
                        exception);
                }
                
                if (!configResponseType.IsAssignableFrom(responseType)) {
                    ValidatorLoadingException exception = new ValidatorLoadingException(validatorType,
                        "One of the ConfigurableWith attributes declared that this validator "               +
                        $"should accept the config of type {configType.Name}, but this config response type " +
                        $"{configResponseType} is not assignable from the validator's target response type " +
                        $"{responseType} and thus, can't be used as a valid configuration type");

                    HandleSoftLoadingException(exception);
                    return GenericLoadingResult<ResponseValidatorInfo>.FromFailure(validatorType.Name,
                        exception);
                }
            
                availableConfigTypes.Add(configType);
            }

            ResponseValidatorInfo info;
            if (useSP) {
                info = ResponseValidatorInfo.WithSP(validatorType, responseType, availableConfigTypes, 
                    serviceProvider);
            } else {
                info = ResponseValidatorInfo.WithNoSP(validatorType, responseType, availableConfigTypes);
            }
        
            _entitiesRegistry.RegisterValidator(info);
            return GenericLoadingResult<ResponseValidatorInfo>.FromSuccess(validatorType.Name, info);
        } catch (Exception e) {
            if (_config.StrictLoadingModeEnabled) {
                throw;
            }
            
            _logger.LogWarning(e, $"Exception occurred when loading the parser {validatorType.Name}");
            return GenericLoadingResult<ResponseValidatorInfo>.FromFailure(validatorType.Name, e);
        }
    }
    
    private void HandleSoftLoadingException(Exception exception)
    {
        _logger.LogWarning(exception.Message);
        if (_config.StrictLoadingModeEnabled) {
            throw exception;
        }
    }
    
    [Pure]
    private static IEnumerable<TypeInfo> SearchModules(Assembly assembly)
    {
        List<TypeInfo> moduleTypes = new List<TypeInfo>();
        foreach (TypeInfo typeInfo in assembly.DefinedTypes) {
            if (!_moduleInterfaceType.IsAssignableFrom(typeInfo)) {
                continue;
            }

            moduleTypes.Add(typeInfo);
        }
        
        return moduleTypes;
    }

    [Pure]
    private static bool IsValidModuleDefinition(Type moduleType)
    {
        return moduleType is {
            IsPublic: true, IsClass: true, IsAbstract: false, IsNested: false,
            ContainsGenericParameters: false,
        };
    }

    [Pure]
    private static bool IsValidHandlerDefinition(MethodInfo methodInfo)
    {
        return methodInfo is {
            IsPublic: true, IsStatic: false, IsAbstract: false,
            ContainsGenericParameters: false
        };
    }

    [Pure]
    private static bool IsValidParserDefinition(Type parserType)
    {
        return parserType is {
            IsPublic: true, IsClass: true, IsAbstract: false,
        };
    }
    
    [Pure]
    private static bool IsValidValidatorDefinition(Type validatorType)
    {
        return validatorType is {
            IsPublic: true, IsClass: true, IsAbstract: false,
        };
    }
}