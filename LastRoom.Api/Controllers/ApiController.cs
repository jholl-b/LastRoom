using FluentResults;
using LastRoom.Api.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LastRoom.Api.Controllers;

[ApiController]
public class ApiController : ControllerBase
{
    protected ActionResult Problem(List<IError> errors)
    {
        if (errors.Count is 0)
        {
            return Problem();
        }

        var modelState = new ModelStateDictionary();

        foreach (var error in errors)
        {
            var code = error.Metadata["ErrorCode"] as string;
            modelState.AddModelError(code ?? "", error.Message);
        }
        
        var errorType = (ErrorType)errors[0].Metadata["ErrorType"];
        var errorDescription = errors[0].Message;

        return Problems(modelState, errorType, errorDescription);
    }
    
    private ActionResult Problems(ModelStateDictionary modelState, ErrorType errorType, string errorDescription)
    {
        var statusCode = errorType switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        return ValidationProblem(
            statusCode: statusCode,
            title: errorDescription,
            modelStateDictionary: modelState
        );
    }
}