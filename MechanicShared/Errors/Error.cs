using FluentValidation.Results;

namespace MechanicShared.Errors;

public record Error(string Message);

public record NotFoundError(string Message) : Error(Message);

public record ValidationError(string Message, List<ValidationFailure> ValidationFailures) : Error(Message);

public record StatusChangeError(string Message) : Error(Message);
