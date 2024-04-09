﻿using PrettyBots.Environment;
using PrettyBots.Model.Responses;

namespace PrettyBots.Interactions.Model.Responses;

/// <inheritdoc />
public class BasicResponseModel<TResponse> : IResponseModel
    where TResponse : IUserResponse
{
    /// <inheritdoc />
    public string Key { get; }

    public Type ResponseType { get; }

    /// <inheritdoc />
    public Type? ResponseParserType { get; set; }
    
    public BasicResponseModel(string key, Type? responseParserType)
    {
        Key                    = key;
        ResponseType           = typeof(TResponse);
        ResponseParserType     = responseParserType;
    }
}
