namespace MechanicShared.Models.Dto;

public class UpdateClient
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
}