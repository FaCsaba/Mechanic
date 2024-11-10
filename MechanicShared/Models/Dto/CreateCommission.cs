namespace MechanicShared.Models.Dto;

public class CreateCommission
{
    public Guid ClientId { get; set; }
    public string LicensePlateNumber { get; set; }
    public DateOnly VehicleManufacturingDate { get; set; }
    public FaultCategory FaultCategory { get; set; }
    public string Description { get; set; }
    public int Severity { get; set; }
}