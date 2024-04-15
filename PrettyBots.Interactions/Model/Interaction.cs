using System.Collections.ObjectModel;

using PrettyBots.Environment;
using PrettyBots.Interactions.Abstraction.Model;
using PrettyBots.Interactions.Abstraction.Model.Responses;

namespace PrettyBots.Interactions.Model;

/// <inheritdoc />
public class Interaction : IInteraction
{
    /// <inheritdoc />
    public uint Id { get; }

    /// <inheritdoc />
    public IReadOnlyList<IResponseModel> AvailableResponses { get; }

    private readonly Dictionary<string, IResponseModel> _responses;

    public Interaction(uint id, IList<IResponseModel> availableResponses)
    {
        Id = id;
        AvailableResponses = 
            new ReadOnlyCollection<IResponseModel>(availableResponses);

        _responses = new Dictionary<string, IResponseModel>(availableResponses.DistinctBy(r => r.Key)
            .Select(r => new KeyValuePair<string, IResponseModel>(r.Key, r)));
    }

    public IResponseModel? GetResponse(string key) => _responses.GetValueOrDefault(key);

    public IValidatableResponseModel? GetValidatableResponse(string key)
    {
        IResponseModel? model = _responses.GetValueOrDefault(key);
        if (model is null) {
            return null;
        }

        if (model is not IValidatableResponseModel vModel) {
            throw new ArgumentException("The response with this key is not a validatable response model");
        }

        return vModel;
    }

    public IValidatableResponseModel<TResponse>? GetValidatableResponse<TResponse>(string key)
        where TResponse : IUserResponse
    {
        IResponseModel? model = _responses.GetValueOrDefault(key);
        if (model is null) {
            return null;
        }

        if (model is not IValidatableResponseModel<TResponse> vModel) {
            throw new ArgumentException("The response with this key is not a validatable response model " +
                                        "with the TResponse provided");
        }

        return vModel;
    }
}   