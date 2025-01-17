using MechanicShared.Errors;
using MechanicShared.Models;
using MechanicShared.Models.Dto;
using MechanicBE.ResultType;

namespace MechanicBE.Services;

public interface IClientService
{
    Task<IEnumerable<Client>> GetAllClientsAsync();

    Task<Client?> GetClientAsync(Guid id);

    Task<Error?> UpdateClientAsync(UpdateClient updateClient);

    Task<Result<Client>> CreateClientAsync(CreateClient createClient);

    Task<Error?> DeleteClientAsync(Guid id);

    Task<Result<Client>> EnsureClientExists(Guid id);
}