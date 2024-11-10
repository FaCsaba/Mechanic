using FluentValidation;
using FluentValidation.Results;
using MechanicBE.Models.Dto;
using MechanicBE.ResultType;
using Microsoft.AspNetCore.Mvc;
using IResult = MechanicBE.ResultType.IResult;

namespace MechanicBE.Errors;

public record Error(string Message)
{
    public static implicit operator ObjectResult(Error? err) => err switch
    {
        null => new OkObjectResult(null),
        NotFoundError error => new NotFoundObjectResult(error),
        _ => new BadRequestObjectResult(err)
    };

    public static implicit operator ActionResult(Error error) => (ObjectResult)error;
}

public record NotFoundError(string Message) : Error(Message);

public record ValidationError(string Message, List<ValidationFailure> ValidationFailures) : Error(Message);

public record StatusChangeError(string Message) : Error(Message);

public static class ErrorExt
{
    public static async Task<Result<T>> EnsureValidAsync<T>(this IValidator<T> validator, T item)
    {
        var validationResult = await validator.ValidateAsync(item);
        if (validationResult.IsValid) return item;
        return new ValidationError("Validation failed", validationResult.Errors);
    }
}