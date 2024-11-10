namespace MechanicBE.Models.Dto;

public class CreateCommission
{
    public Guid ClientId { get; init; }
    public string LicensePlateNumber { get; init; }
    public DateOnly VehicleManufacturingDate { get; init; }
    public FaultCategory FaultCategory { get; init; }
    public string Description { get; init; }
    public int Severity { get; init; }
}