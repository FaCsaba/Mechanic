using FluentValidation.Results;
using MechanicShared.Errors;

namespace MechanicFE;

public static class ValidationErrorExt
{
    public static IEnumerable<string> ExtractErrors(this List<ValidationFailure> failures, string property) =>
        failures.FindAll(x => x.PropertyName == property).Select(x => x.ErrorMessage);
}