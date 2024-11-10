using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MechanicShared.Models;

public class Commission
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }

    public Guid ClientId { get; set; }

    public Client Client { get; init; }

    [Length(7, 7)] public string LicensePlateNumber { get; set; }

    public DateOnly VehicleManufacturingDate { get; set; }

    public FaultCategory FaultCategory { get; set; }

    [Length(0, 256)] public string? Description { get; set; }

    public int Severity { get; set; }

    public CommissionStatus Status { get; set; } = CommissionStatus.Todo;
}