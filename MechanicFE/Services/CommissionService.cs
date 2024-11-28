using System.Net.Http.Json;
using MechanicFE.Exceptions;
using MechanicShared.Errors;
using MechanicShared.Models.Dto;

namespace MechanicFE.Services;

public class CommissionService(HttpClient client) : ICommissionService
{
    public async Task<IQueryable<CommissionDto>> GetCommissions()
    {
        var commissionDtos = await client.GetFromJsonAsync<List<CommissionDto>>("commission")
                             ?? throw new Exception("Failed to get commissions");
        return commissionDtos.AsQueryable();
    }
    
    public async Task DeleteCommission(Guid id)
    {
        await client.DeleteAsync($"commission?id={id}");
    }
    
    public async Task CreateCommission(CreateCommission createCommission)
    {
        var res = await client.PostAsJsonAsync("commission", createCommission);
        if (!res.IsSuccessStatusCode)
        {
            var validationError = await res.Content.ReadFromJsonAsync<ValidationError>()
                                  ?? throw new Exception("Failed to read error message");
            throw new ValidationErrorException(validationError.ValidationFailures);
        }
    }

    public async Task UpdateCommission(UpdateCommission updateCommission)
    {
        var res = await client.PutAsJsonAsync("commission", updateCommission);
        if (!res.IsSuccessStatusCode)
        {
            var validationError = await res.Content.ReadFromJsonAsync<ValidationError>()
                                  ?? throw new Exception("Failed to read error message");
            throw new ValidationErrorException(validationError.ValidationFailures);
        }   
    }
}