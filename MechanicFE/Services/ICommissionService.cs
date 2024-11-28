using MechanicShared.Models.Dto;

namespace MechanicFE.Services;

public interface ICommissionService
{
    Task<IQueryable<CommissionDto>> GetCommissions();

    Task DeleteCommission(Guid id);

    Task CreateCommission(CreateCommission createCommission);

    Task UpdateCommission(UpdateCommission updateCommission);
}