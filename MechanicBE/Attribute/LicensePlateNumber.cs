using System.ComponentModel.DataAnnotations;

namespace MechanicBE.Attribute;

public class LicensePlateNumber : ValidationAttribute
{
    public override bool IsValid(object? value)
        => value is string licensePlateNumber && IsValid(licensePlateNumber);

    
}