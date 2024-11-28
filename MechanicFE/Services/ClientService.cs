using System.Net.Http.Json;
using MechanicFE.Exceptions;
using MechanicShared.Errors;
using MechanicShared.Models;
using MechanicShared.Models.Dto;

namespace MechanicFE.Services;

public class ClientService(HttpClient client) : IClientService
{
    public async Task<IQueryable<Client>> GetClients()
    {
        var clients = await client.GetFromJsonAsync<List<Client>>("client") ?? throw new Exception("Failed to get clients");
        return clients.AsQueryable();   
    }

    public async Task DeleteClient(Guid clientId)
    {
        await client.DeleteAsync($"client/{clientId}");
    }

    public async Task CreateClient(CreateClient createClient)
    {
        
        var res = await client.PostAsJsonAsync("client", createClient);
        if (!res.IsSuccessStatusCode)
        {
            var validationError = await res.Content.ReadFromJsonAsync<ValidationError>()
                                  ?? throw new Exception("Failed to read error message");
            throw new ValidationErrorException(validationError.ValidationFailures);
        }
    }

    public async Task UpdateClient(UpdateClient updateClient)
    {
        var res = await client.PutAsJsonAsync("client", updateClient);
        if (!res.IsSuccessStatusCode)
        {
            var validationError = await res.Content.ReadFromJsonAsync<ValidationError>()
                                  ?? throw new Exception("Failed to read error message");
            throw new ValidationErrorException(validationError.ValidationFailures);
        }
    }
}