using FluentValidation;
using MechanicBE.Contexts;
using MechanicBE.Errors;
using MechanicBE.Models;
using MechanicBE.Models.Dto;
using MechanicBE.ResultType;
using Microsoft.EntityFrameworkCore;

namespace MechanicBE.Services;

public class CommissionService(
    MechanicContext db,
    IClientService clientService,
    IValidator<CreateCommission> createCommissionValidator,
    IValidator<UpdateCommission> updateCommissionValidator)
    : ICommissionService
{
    public async Task<List<Commission>> GetCommissionsAsync() =>
        await db.Commissions.Include(x => x.Client).ToListAsync();

    public async Task<Result<Commission>> CreateCommissionAsync(CreateCommission createCommission)
    {
        var validationRes = await createCommissionValidator.EnsureValidAsync(createCommission);
        return await validationRes.MapAsync(async commissionDto =>
        {
            var commission = new Commission
            {
                ClientId = commissionDto.ClientId,
                FaultCategory = commissionDto.FaultCategory,
                Severity = commissionDto.Severity,
                VehicleManufacturingDate = commissionDto.VehicleManufacturingDate,
                Status = CommissionStatus.Todo,
                LicensePlateNumber = commissionDto.LicensePlateNumber,
                Description = commissionDto.Description
            };
            await db.Commissions.AddAsync(commission);
            await db.SaveChangesAsync();
            return commission;
        });
    }

    public async Task<Error?> DeleteCommissionAsync(Guid id)
    {
        var commissionRes = await EnsureCommissionExists(id);
        return await commissionRes.UseAsync(async c =>
        {
            db.Remove(c);
            await db.SaveChangesAsync();
        });
    }

    public async Task<Error?> UpdateCommissionAsync(UpdateCommission updateCommission)
    {
        var validationRes = await updateCommissionValidator.EnsureValidAsync(updateCommission);
        Result<Client> validateClientRes = await validationRes.MapAsync(_ =>
            clientService.EnsureClientExists(updateCommission.ClientId));
        Result<Commission> commissionRes =
            await validateClientRes.MapAsync(_ => EnsureCommissionExists(updateCommission.Id));
        Result<Commission> statusChangeRes = commissionRes.Map<Result<Commission>>(c =>
        {
            var statusChangeValidation = ValidateStatusChange(c.Status, updateCommission.Status);
            if (statusChangeValidation is null) return c;
            return statusChangeValidation;
        });

        return await statusChangeRes.UseAsync(async c =>
        {
            c.ClientId = updateCommission.ClientId;
            c.FaultCategory = updateCommission.FaultCategory;
            c.Severity = updateCommission.Severity;
            c.VehicleManufacturingDate = updateCommission.VehicleManufacturingDate;
            c.Status = updateCommission.Status;
            c.LicensePlateNumber = updateCommission.LicensePlateNumber;
            c.Description = updateCommission.Description;
            await db.SaveChangesAsync();
        });
    }

    public async Task<Result<Commission>> EnsureCommissionExists(Guid id)
    {
        var commission = await db.Commissions.Include(x => x.Client).Where(x => x.Id == id).FirstOrDefaultAsync();
        if (commission is null) return new NotFoundError("Commission with id could not be found");
        return commission;
    }

    private StatusChangeError? ValidateStatusChange(CommissionStatus commissionStatus,
        CommissionStatus updateCommissionStatus)
    {
        if (commissionStatus == updateCommissionStatus || commissionStatus + 1 == updateCommissionStatus) return null;
        return new StatusChangeError("Changing status to " + updateCommissionStatus + " is incorrect!");
    }
}