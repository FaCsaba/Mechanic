namespace MechanicBE.Models.Dto;

public class CommissionDto
{
    public required Guid Id { get; init; }
    public required Client Client { get; init; }
    public required string LicensePlateNumber { get; init; }
    public required DateOnly VehicleManufacturingDate { get; init; }
    public required FaultCategory FaultCategory { get; init; }
    public required string? Description { get; init; }
    public required int Severity { get; init; }
    public required CommissionStatus Status { get; init; }
    public required double WorkHourEstimate { get; init; }
}