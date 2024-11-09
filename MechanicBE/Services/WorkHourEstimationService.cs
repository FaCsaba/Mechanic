using System.Diagnostics;
using MechanicBE.Models;

namespace MechanicBE.Services;

public class WorkHourEstimationService(TimeProvider timeProvider) : IWorkHourEstimator
{
    public double WorkHours(Commission commission) =>
        WorkHoursFromFaultCategory(commission.FaultCategory) *
        WorkHourWeightFromAge(commission.VehicleManufacturingDate) *
        WorkHourWeightFromSeverity(commission.Severity);

    private static double WorkHoursFromFaultCategory(FaultCategory faultCategory) => faultCategory switch
    {
        FaultCategory.Bodywork => 3,
        FaultCategory.Motor => 8,
        FaultCategory.RunningGear => 6,
        FaultCategory.Brake => 4,
        _ => throw new ArgumentOutOfRangeException(nameof(faultCategory), faultCategory, null)
    };

    private double WorkHourWeightFromAge(DateOnly manufacturingDate) =>
        GetAgeFromDate(manufacturingDate) switch
        {
            >= 0 and < 5 => 0.5,
            >= 5 and < 10 => 1,
            >= 10 and < 20 => 1.5,
            >= 20 => 2,
            _ => throw new UnreachableException("Age should never be lower than 0.")
        };

    private int GetAgeFromDate(DateOnly date) =>
        timeProvider.GetUtcNow().Year - date.Year; // TODO: this is really not ok

    private static double WorkHourWeightFromSeverity(int severity) => severity switch
    {
        1 or 2 => 0.2,
        2 or 4 => 0.4,
        >= 5 and <= 7 => 0.6,
        8 or 9 => 0.8,
        10 => 1,
        _ => throw new UnreachableException("Severity should never be outside of the 1-10 range")
    };
}