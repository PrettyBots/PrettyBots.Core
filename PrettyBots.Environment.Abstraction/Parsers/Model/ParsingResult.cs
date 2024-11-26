using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PrettyBots.Environment.Parsers.Model;

public class ParsingResult
{
    [MemberNotNullWhen(false, nameof(ErrorMessage))]
    [MemberNotNullWhen(false, nameof(ErrorType))]
    public bool Success { get; }
    public ParsingErrorType? ErrorType { get; }
    public string? ErrorMessage { get; }
    public IUserResponse?  Response { get; }
    public bool HandleStopRequested { get; }

    private ParsingResult(bool success, IUserResponse? response, ParsingErrorType? errorType, string? errorMessage, bool handleStopRequested)
    {
        Success = success;
        Response = response;
        ErrorType = errorType;
        ErrorMessage = errorMessage;
        HandleStopRequested = handleStopRequested;
    }

    public static ParsingResult Ok(IUserResponse response)
    {
        return new ParsingResult(true, response, null, null, false);
    }

    public static ParsingResult Error(ParsingErrorType errorType, string error)
    {
        return new ParsingResult(false, null, errorType, error, false);
    }

    public static ParsingResult StopHandle()
    {
        return new ParsingResult(true, null, null, null, true);
    }
}