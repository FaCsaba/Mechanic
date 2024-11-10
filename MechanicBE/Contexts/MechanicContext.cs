using MechanicShared.Models;
using Microsoft.EntityFrameworkCore;

namespace MechanicBE.Contexts;

public class MechanicContext(DbContextOptions<MechanicContext> options) : DbContext(options)
{
    public DbSet<Commission> Commissions { get; set; }
    public DbSet<Client> Clients { get; set; }
}