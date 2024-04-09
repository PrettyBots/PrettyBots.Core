﻿using PrettyBots.Environment;
using PrettyBots.Model.Responses;

namespace PrettyBots.Interactions.Builders.InteractionResponses;

/// <summary>
/// Universal interface for response builders.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IResponseModelBuilder<in TResponse>
    where TResponse : IUserResponse
{
    /// <summary>
    /// Builds the generic response model.
    /// </summary>
    /// <returns></returns>
    IResponseModel Build();
}