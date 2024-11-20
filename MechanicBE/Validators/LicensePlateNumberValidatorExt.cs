using FluentValidation;

namespace MechanicBE.Validators;

public static class LicensePlateNumberValidatorExt
{
    public static IRuleBuilderOptions<T, string?> LicensePlateNumber<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder.Must(IsValidLicensePlateNumber).WithMessage("Is not a license plate number.");
    }

    private static bool IsValidLicensePlateNumber(string? licensePlateNumber) =>
        licensePlateNumber is null || licensePlateNumber.Length == 7 && licensePlateNumber[..3].All(c =>
            char.IsAscii(c) && char.IsUpper(c)) &&
        licensePlateNumber[3] == '-' &&
        licensePlateNumber[^3..].All(char.IsNumber);
}