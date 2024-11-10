namespace MechanicShared.Models.Dto;

public class UpdateCommission
{
    public required Guid Id { get; init; }
    public required Guid ClientId { get; init; }
    public required string LicensePlateNumber { get; init; }
    public required DateOnly VehicleManufacturingDate { get; init; }
    public required FaultCategory FaultCategory { get; init; }
    public required string Description { get; init; }
    public required int Severity { get; init; }
    public required CommissionStatus Status { get; init; }
}