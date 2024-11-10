using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MechanicShared.Models;

public class Client
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [MaxLength(256)] public required string Name { get; set; }

    [MaxLength(256)] public required string Address { get; set; }

    [MaxLength(256)] public required string Email { get; set; }
}