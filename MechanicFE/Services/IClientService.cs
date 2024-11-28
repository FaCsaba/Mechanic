using MechanicShared.Models;
using MechanicShared.Models.Dto;

namespace MechanicFE.Services;

public interface IClientService
{
    Task<IQueryable<Client>> GetClients();

    Task DeleteClient(Guid clientId);

    Task CreateClient(CreateClient createClient);

    Task UpdateClient(UpdateClient updateClient);
}