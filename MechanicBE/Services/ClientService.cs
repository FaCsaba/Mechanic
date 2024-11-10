using FluentValidation;
using MechanicBE.Contexts;
using MechanicBE.Errors;
using MechanicBE.Models;
using MechanicBE.Models.Dto;
using MechanicBE.ResultType;
using Microsoft.EntityFrameworkCore;

namespace MechanicBE.Services;

public class ClientService(
    MechanicContext db,
    IValidator<CreateClient> createClientValidator,
    IValidator<UpdateClient> updateClientValidator) : IClientService
{
    public async Task<IEnumerable<Client>> GetAllClientsAsync() => await db.Clients.ToListAsync();

    public async Task<Client?> GetClientAsync(Guid id) => await db.Clients.FindAsync(id);

    public async Task<Error?> UpdateClientAsync(UpdateClient updateClient)
    {
        var validationResult = await updateClientValidator.EnsureValidAsync(updateClient);

        Result<Client> clientResult =
            await validationResult.MapAsync(c => EnsureClientExists(c.Id));

        return await clientResult.UseAsync(async client =>
        {
            client.Address = updateClient.Address;
            client.Email = updateClient.Email;
            client.Name = updateClient.Name;
            await db.SaveChangesAsync();
        });
    }

    public async Task<Result<Client>> CreateClientAsync(CreateClient createClientDto)
    {
        var validationResult = await createClientValidator.EnsureValidAsync(createClientDto);
        return await validationResult.MapAsync(async createClient =>
        {
            var client = new Client
                { Name = createClient.Name, Address = createClient.Address, Email = createClient.Email };
            await db.Clients.AddAsync(client);
            await db.SaveChangesAsync();
            return client;
        });
    }

    public async Task<Error?> DeleteClientAsync(Guid id)
    {
        var clientResult = await EnsureClientExists(id);
        return await clientResult.UseAsync(async c =>
        {
            db.Remove(c);
            await db.SaveChangesAsync();
        });
    }

    public async Task<Result<Client>> EnsureClientExists(Guid id)
    {
        var client = await GetClientAsync(id);
        if (client is null) return new NotFoundError("Client with id could not be found");
        return client;
    }
}