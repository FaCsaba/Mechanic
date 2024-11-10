using FluentValidation;
using MechanicBE.ResultType;
using MechanicShared.Errors;
using Microsoft.AspNetCore.Mvc;

namespace MechanicBE.Errors;

public static class ErrorExt
{
    public static ObjectResult ToObjectResult(this Error? err)  => err switch
    {
        null => new OkObjectResult(null),
        NotFoundError error => new NotFoundObjectResult(error),
        _ => new BadRequestObjectResult(err)
    };

    public static async Task<Result<T>> EnsureValidAsync<T>(this IValidator<T> validator, T item)
    {
        var validationResult = await validator.ValidateAsync(item);
        if (validationResult.IsValid) return item;
        return new ValidationError("Validation failed", validationResult.Errors);
    }
}