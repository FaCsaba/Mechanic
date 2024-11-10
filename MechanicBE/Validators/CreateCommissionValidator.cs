using FluentValidation;
using MechanicShared.Models.Dto;

namespace MechanicBE.Validators;

public class CreateCommissionValidator : AbstractValidator<CreateCommission>
{
    public CreateCommissionValidator()
    {
        RuleFor(x => x.ClientId).NotNull();
        RuleFor(x => x.LicensePlateNumber).NotNull().LicensePlateNumber();
        RuleFor(x => x.VehicleManufacturingDate).NotNull().GreaterThan(new DateOnly(1999, 1, 1));
        RuleFor(x => x.FaultCategory).NotNull();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Severity).InclusiveBetween(1, 10);
    }
}