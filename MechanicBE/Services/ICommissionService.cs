using MechanicShared.Errors;
using MechanicShared.Models;
using MechanicShared.Models.Dto;
using MechanicBE.ResultType;

namespace MechanicBE.Services;

public interface ICommissionService
{
    Task<List<Commission>> GetCommissionsAsync();
    Task<Result<Commission>> CreateCommissionAsync(CreateCommission createCommission);
    Task<Error?> DeleteCommissionAsync(Guid id);
    Task<Error?> UpdateCommissionAsync(UpdateCommission updateCommission);
    Task<Result<Commission>> EnsureCommissionExists(Guid id);
}