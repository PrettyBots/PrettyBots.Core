using System.Net.Http.Json;
using System.Text.Json.Serialization;

using Newtonsoft.Json;

using PrettyBots.Environment;
using PrettyBots.Storages.Abstraction.Model;

namespace PrettyBots.Interactions.Abstraction.Model.Context;

/// <inheritdoc />
public class InteractionContext<TMessage, TUser, TResponse> : IInteractionContext<TMessage, TUser, TResponse>
    where TMessage : IUserMessage
    where TUser : class, IUser
    where TResponse : IUserResponse
{
    /// <inheritdoc />
    public IInteractionService InteractionService { get; }
    
    /// <inheritdoc />
    public IInteraction TargetInteraction { get; }

    /// <inheritdoc />
    public TUser User { get; }

    /// <inheritdoc />
    public string ResponseKey { get; }

    public string? InteractionDataString
    {
        get => User.InteractionData;
        set {
            DataChanged = true;
            User.InteractionData = value;
        }
    }

    public bool DataChanged { get; set; } = false;

    private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings() {
        NullValueHandling = NullValueHandling.Ignore,
    };

    public TData? GetInteractionData<TData>() 
        where TData : class
    {
        return InteractionDataString != null ? JsonConvert.DeserializeObject<TData>(InteractionDataString, _serializerSettings) : null;
    }

    public void SetInteractionData(object? data)
    {
        InteractionDataString = data is null ? null : JsonConvert.SerializeObject(data, _serializerSettings);
    }

    /// <inheritdoc />
    public TResponse Response { get; }

    public TMessage OriginalMessage { get; }

    public InteractionContext(IInteractionService interactionService, 
        IInteraction targetInteraction, string responseKey,
        TResponse response, TMessage originalMessage, TUser user)
    {
        User               = user;
        Response           = response;
        ResponseKey        = responseKey;
        OriginalMessage    = originalMessage;
        TargetInteraction  = targetInteraction;
        InteractionService = interactionService;
    }
}