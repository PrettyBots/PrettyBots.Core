using PrettyBots.Environment;
using PrettyBots.Interactions.Abstraction.Model.Descriptors;
using PrettyBots.Interactions.Abstraction.Model.Responses;
using PrettyBots.Validators.Abstraction;
using PrettyBots.Validators.Abstraction.Model;

namespace PrettyBots.Interactions.Abstraction.Model.Context.Errors;

public class ResponseValidationResult
{
    public IResponseModel     ResponseModel  { get; }
    public IResponseValidator Validator { get; }
    public ValidationResult   Result    { get; }
    public IUserResponse      Response  { get; }

    public ResponseValidationResult(IResponseModel responseModel, IResponseValidator validator, ValidationResult result,
        IUserResponse response)
    {
        ResponseModel = responseModel;
        Validator     = validator;
        Result        = result;
        Response      = response;
    }
}