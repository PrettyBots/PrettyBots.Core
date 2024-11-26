using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using JetBrains.Annotations;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using PrettyBots.Environment;
using PrettyBots.Environment.Parsers.Model;
using PrettyBots.Interactions.Abstraction;
using PrettyBots.Interactions.Abstraction.EventHandlers;
using PrettyBots.Interactions.Abstraction.Model.Descriptors;
using PrettyBots.Interactions.Abstraction.Model.Responses;
using PrettyBots.Interactions.Abstraction.Services;
using PrettyBots.Interactions.Exceptions;
using PrettyBots.Interactions.Exceptions.Handling;
using PrettyBots.Interactions.Utilities.DependencyInjection;
using PrettyBots.Interactions.Validators;
using PrettyBots.Storages.Abstraction;
using PrettyBots.Utilities.Collections;
using PrettyBots.Interactions.Abstraction.Model.Context;
using PrettyBots.Interactions.Abstraction.Model.Context.Errors;
using PrettyBots.Interactions.Validators.Abstraction.Model;
using PrettyBots.Interactions.Validators.Text;
using PrettyBots.Storages.Abstraction.Model;

namespace PrettyBots.Interactions;

public class InteractionService<TMessage> : IInteractionService, 
                                            IMessageHandler<TMessage>
    where TMessage : class, IUserMessage
{
    /// <inheritdoc />
    public IEntitiesLoader Loader { get; private set; }
    
    /// <inheritdoc />
    public ILoadedEntitiesRegistry Registry { get; private set; }

    public IConfigurationService Config { get; private set; }
    
    public IEnvironment Environment { get; private set; }
    public event IncorrectUserMessageEventHandler? OnIncorrectUserMessage;

    
    private ILogger<InteractionService<TMessage>> _logger;
    
    private IStorageProvider _storage = null!;
    private SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

    public InteractionService(IEnvironment environment)
    {
        InternalInit(environment: environment);
    }
    
    [UsedImplicitly]
    public InteractionService(ILogger<InteractionService<TMessage>> logger, IEnvironment environment, 
        IEntitiesLoader loader, ILoadedEntitiesRegistry registry, IConfigurationService config, IStorageProvider storage)
    {
        InternalInit(environment, logger, loader, registry, config, storage);
    }

    [MemberNotNull(nameof(Loader))]
    [MemberNotNull(nameof(Registry))]
    [MemberNotNull(nameof(Config))]
    [MemberNotNull(nameof(_logger))]
    [MemberNotNull(nameof(Environment))]
    private void InternalInit(IEnvironment environment, ILogger<InteractionService<TMessage>>? logger = null,
        IEntitiesLoader? loader = null, ILoadedEntitiesRegistry? registry = null, IConfigurationService? config = null, 
        IStorageProvider? storage = null)
    {
        IServiceProvider provider = DefaultServiceProvider
            .BuildDefaultServiceProvider(environment);

        if (!typeof(TMessage).IsEquivalentTo(environment.MessageType)) {
            throw new CriticalServiceException("The interaction service was made to " + 
                $"handle messages of the {typeof(TMessage)} type, but environment has set the " +
                $"message type to {environment.MessageType}");
        }
        
        Environment = environment;
        _logger     = logger   ?? provider.GetRequiredService<ILogger<InteractionService<TMessage>>>();
        _storage    = storage  ?? provider.GetRequiredService<IStorageProvider>();
        Config      = config   ?? provider.GetRequiredService<IConfigurationService>(); 
        Loader      = loader   ?? provider.GetRequiredService<IEntitiesLoader>();
        Registry    = registry ?? provider.GetRequiredService<ILoadedEntitiesRegistry>();

        bool strictTemp = Config.StrictLoadingModeEnabled;
        Config.StrictLoadingModeEnabled = true;
        Loader.LoadResponseValidators(Assembly.GetAssembly(typeof(RichTextResponseValidator))!);
        Config.StrictLoadingModeEnabled = strictTemp;
    }

    public async Task LaunchInteractionAsync(long userId, uint interactionId, CancellationToken token = default)
    {
        await _storage.StoreInteractionIdAsync(userId, interactionId, token);
    }

    public async Task HandleUserMessageAsync(TMessage message, CancellationToken token = default)
    {
        await _lock.WaitAsync(token).ConfigureAwait(false);
        
        
        //Happy house starting
        try {
            IUser? user = await _storage.RetrieveUserDataAsync(message.UserId, token);
            if (user is null) {
                _logger.LogWarning(new InteractionNotRegisteredException(), 
                    $"User with id = {message.UserId} is not represented in storage");
                return;
                //throw new InteractionNotStartedException();
            }

            if (!Registry.Interactions.TryGetValue(user.CurrentInteractionId, out InteractionInfo? currentInteraction)) {
                throw new InteractionNotRegisteredException();
            }

            List<ResponseValidationResult> validationResults = new List<ResponseValidationResult>();
            IInteractionContext<TMessage, IUser, IUserResponse> interactionContext;

            bool successfulValidationAchieved = false;
            
            foreach (IResponseModel availableResponse in currentInteraction.Interaction.AvailableResponses) {
                DefaultEntityCollection<ResponseParserInfo>? responseParsers =   
                    Registry.ResponseParsers[availableResponse.ResponseType!];
                
                ResponseParserInfo responseParserInfo = responseParsers.First((responseParser) => 
                    responseParser.ParserType.IsEquivalentTo(availableResponse.ResponseParserType));
                
                if (!responseParserInfo.Instance.CanParse(message)) {
                    continue;
                }
                
                ParsingResult result = await responseParserInfo.Instance.ParseResponseAsync(message, token)
                                                             .ConfigureAwait(false);
                
                if (!result.Success) {
                    continue;
                }

                if (result.HandleStopRequested) {
                    return;    
                }
                
                result.Response!.Environment = Environment;
                if (availableResponse is IValidatableResponseModel validatableResponse) {
                    ValidationResult validateResult = await validatableResponse.Validator
                                          .ValidateResponseAsync(result.Response!, validatableResponse.Config!)
                                         .ConfigureAwait(false);
                    
                    if (!validateResult.Success) {
                        validationResults.Add(new ResponseValidationResult(availableResponse, 
                            validatableResponse.Validator, validateResult, result.Response!));
                        continue;
                    }
                }

                interactionContext =
                    new InteractionContext<TMessage, IUser, IUserResponse>(
                        this, currentInteraction.Interaction, availableResponse.Key,
                        result.Response!, message, user);

                await currentInteraction.HandlerInfo!.ExecutionContext.Execute(interactionContext, token: token).ConfigureAwait(false);

                if (interactionContext.DataChanged) {
                    await _storage.StoreInteractionDataAsync(user.TelegramUserId, user.InteractionData, token).ConfigureAwait(false);
                }

                successfulValidationAchieved = true;
                break;
            }
            
            if (!successfulValidationAchieved && OnIncorrectUserMessage is not null) {
                await OnIncorrectUserMessage.Invoke(
                    IncorrectUserMessageErrorContext.FromNoSuccessfulValidator(this, 
                        currentInteraction.Interaction, message, validationResults));
            }
        } catch (Exception e) {
            HandleSoftHandlingException(e);
            return;
        } finally {
            _lock.Release();
        }
        
    }


    private void HandleSoftHandlingException(Exception exception)
    {
        _logger.LogWarning(exception.Message);
        if (Config.StrictHandlingModeEnabled) {
            throw exception;
        }
    }
}