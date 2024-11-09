using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MechanicBE.Attribute;

namespace MechanicBE.Models;

public class Commission
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }
    
    public Guid ClientId { get; set; }
    public Client Client { get; set; }

    [LicensePlateNumber] [Length(7, 7)] public string LicensePlateNumber { get; set; }

    public DateOnly VehicleManufacturingDate { get; set; }

    public FaultCategory FaultCategory { get; set; }

    public int Severity { get; set; }

    public CommissionStatus Status { get; set; }
}