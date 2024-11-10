using FluentValidation;
using MechanicShared.Models.Dto;

namespace MechanicBE.Validators;

public class CreateClientValidator : AbstractValidator<CreateClient>
{
    public CreateClientValidator()
    {
        RuleFor(x => x.Email).NotNull().Length(3, 256).EmailAddress();
        RuleFor(x => x.Name).NotNull().Length(3, 256);
        RuleFor(x => x.Address).NotNull().Length(3, 256);
    }
}