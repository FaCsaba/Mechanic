using FluentValidation.Results;

namespace MechanicFE.Exceptions;

public class ValidationErrorException(List<ValidationFailure> validationFailures) : Exception
{
    public List<ValidationFailure> ValidationFailures { get; set; } = validationFailures;
}