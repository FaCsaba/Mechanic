using FluentValidation;
using MechanicShared.Models.Dto;

namespace MechanicBE.Validators;

public class UpdateClientValidator : AbstractValidator<UpdateClient>
{
    public UpdateClientValidator()
    {
        RuleFor(x => x.Id).NotNull();
        RuleFor(x => x.Email).NotNull().Length(3, 256).EmailAddress();
        RuleFor(x => x.Name).NotNull().Length(3, 256);
        RuleFor(x => x.Address).NotNull().Length(3, 256);
    }
}